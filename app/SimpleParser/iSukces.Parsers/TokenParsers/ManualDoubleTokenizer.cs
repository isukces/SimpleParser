using System;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace iSukces.Parsers.TokenParsers
{
    public class ManualDoubleTokenizer : ValueTokenizer
    {
        public ManualDoubleTokenizer(NumerFlags flags, params char[] decimalSeparators)
        {
            _decimalSeparators  = string.Join("", decimalSeparators);
            _separatorsToChange = decimalSeparators.Where(a => a != '.').ToArray();

            _leadingSpaceAction = (flags & NumerFlags.RequireAtLeastOneLeadingSpace) != 0
                ? SpaceActions.MustHave
                : (flags & NumerFlags.AllowLedingSpaces) != 0
                    ? SpaceActions.Ignore
                    : SpaceActions.Forbidden;

            _allowInts = (flags & NumerFlags.AllowParseInteger) != 0;
        }

        public ManualDoubleTokenizer(NumerFlags flags)
            : this(flags, '.')
        {
        }

        public override TokenCandidate Parse(string text)
        {
            var tmp = new InternalParser(text, this);
            tmp.Parse();
            return tmp.Result;
        }


        private readonly char[] _separatorsToChange;
        private readonly string _decimalSeparators;
        private readonly SpaceActions _leadingSpaceAction;
        private readonly bool _allowInts;

        private enum SpaceActions
        {
            Forbidden,
            Ignore,
            MustHave
        }

        private enum Stages
        {
            S01_SkipLeadingSpaces,
            S02_PlusMinus,
            S03_Digits,
            S04_Fractional,
            S05_ExponentPlusMinus,
            S06_ExponentDigits
        }

        private class InternalParser
        {
            public InternalParser(string text, ManualDoubleTokenizer owner)
            {
                _text  = text;
                _owner = owner;
            }

            public void Parse()
            {
                _stage = _owner._leadingSpaceAction == SpaceActions.Forbidden
                    ? Stages.S02_PlusMinus
                    : Stages.S01_SkipLeadingSpaces;
                for (_index = 0; _index < _text.Length; _index++)
                {
                    _char = _text[_index];
                    switch (_stage)
                    {
                        case Stages.S01_SkipLeadingSpaces:
                            if (Process_S01_SkipLeadingSpaces())
                                return;
                            break;
                        case Stages.S02_PlusMinus:
                            Process_S02_PlusMinus();
                            break;
                        case Stages.S03_Digits:
                            if (Process_S03_Digits())
                                return;
                            break;
                        case Stages.S04_Fractional:
                            if (Process_S04_Fractional())
                                return;
                            break;
                        case Stages.S05_ExponentPlusMinus:
                            Process_S05_ExponentPlusMinus();
                            break;
                        case Stages.S06_ExponentDigits:
                            if (Process_S06_ExponentDigits())
                                return;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                switch (_stage)
                {
                    case Stages.S03_Digits:
                        if (_owner._allowInts)
                            if (_hasIntDigits)
                                Accept();
                        break;
                    case Stages.S04_Fractional:
                        if (_hasFractionalDigits)
                            Accept();
                        break;
                }
            }

            private void Accept()
            {
                if (_resultStart < 0)
                    return;
                var length = _index - _resultStart;
                if (length < 0)
                    return;
                var q = _text.Substring(_resultStart, length);
                if (_actualDecimalSeparator != (char)0 && _actualDecimalSeparator != '.')
                    q = q.Replace(_actualDecimalSeparator, '.');
                var value = double.Parse(q, NumberStyles.Any, CultureInfo.InvariantCulture);
                Result = new TokenCandidate(value, _index, 100);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private bool Process_S01_SkipLeadingSpaces()
            {
                if (_char == ' ')
                    return _owner._leadingSpaceAction == SpaceActions.Forbidden;

                _resultStart = _index;
                if (_owner._leadingSpaceAction == SpaceActions.MustHave && _index == 0)
                    return true;
                _index--;
                _stage = Stages.S02_PlusMinus;
                return false;
            }

            private void Process_S02_PlusMinus()
            {
                if (_char != '+' && _char != '-')
                    _index--;
                else
                {
                    if (_resultStart < 0)
                        _resultStart = _index;
                }

                _stage = Stages.S03_Digits;
            }

            private bool Process_S03_Digits()
            {
                if (_char >= '0' && _char <= '9')
                {
                    if (_resultStart < 0)
                        _resultStart = _index;
                    _hasIntDigits = true;
                    return false;
                }

                if (!_hasIntDigits)
                    return true;
                if (_char == 'e' || _char == 'E')
                {
                    _stage = Stages.S05_ExponentPlusMinus;
                    return false;
                }

                if (_owner._decimalSeparators.IndexOf(_char) >= 0)
                {
                    _actualDecimalSeparator = _char;
                    _stage                  = Stages.S04_Fractional;
                    return false;
                }

                if (_owner._allowInts)
                    Accept();
                return true;
            }

            private bool Process_S04_Fractional()
            {
                if (_char >= '0' && _char <= '9')
                {
                    _hasFractionalDigits = true;
                    return false;
                }

                if (!_hasFractionalDigits)
                    return true;

                if (_char == 'e' || _char == 'E')
                {
                    _stage = Stages.S05_ExponentPlusMinus;
                    return false;
                }

                Accept();
                return true;
            }

            private void Process_S05_ExponentPlusMinus()
            {
                if (_char != '+' && _char != '-')
                    _index--;
                _stage = Stages.S06_ExponentDigits;
            }

            private bool Process_S06_ExponentDigits()
            {
                if (_char >= '0' && _char <= '9')
                {
                    _hasExpDigits = true;
                    return false;
                }

                if (_hasExpDigits)
                    Accept();

                return true;
            }

            public TokenCandidate Result { get; set; }

            private char _char;
            private int _index;
            private Stages _stage;
            private int _resultStart = -1;


            private readonly string _text;
            private readonly ManualDoubleTokenizer _owner;
            private bool _hasIntDigits;
            private bool _hasFractionalDigits;
            private bool _hasExpDigits;
            private char _actualDecimalSeparator = (char)0;
        }
    }
}