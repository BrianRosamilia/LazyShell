using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Windows.Forms;
using LAZYSHELL.Properties;
namespace LAZYSHELL
{
    [Serializable()]
    public sealed class TextHelper
    {
        static TextHelper instance = null;
        static readonly object padlock = new object();
        private bool error = false; public bool Error { get { return this.error; } }
        private Settings settings = Settings.Default;

        TextHelper()
        {

        }
        public static TextHelper Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                        instance = new TextHelper();
                    return instance;
                }
            }
        }

        // STRINGS (57)
        private const string code00 = "endInput"; // SURROUNDED BY {   }
        private const string code01 = "newLine"; // SURROUNDED BY {   }
        private const string code02 = "newLineInput"; // SURROUNDED BY {   }
        private const string code03 = "newPageInput"; // SURROUNDED BY {   }
        private const string code04 = "newPage"; // SURROUNDED BY {   }
        private const string code05 = "pauseInput"; // SURROUNDED BY {   }
        private const string code06 = "end"; // SURROUNDED BY {   }
        private const string code07 = "option"; // SURROUNDED BY {   }
        private const string code08 = "  "; // Text
        private const string code09 = "   "; // Text
        private const string code0A = "    "; // Text
        private const string code0C = "delay"; // SURROUNDED BY {   }
        private const string code0D = "delay..."; // SURROUNDED BY {   }
        private const string code0E = " the"; // Text
        private const string code0F = " you"; // Text
        private const string code10 = "in"; // Text
        private const string code11 = " to "; // Text
        private const string code12 = "'s "; // Text
        private const string code13 = "Mario"; // Text
        private const string code14 = " I "; // Text
        private const string code15 = " and "; // Text
        private const string code16 = "is "; // Text
        private const string code17 = " so"; // Text
        private const string code18 = "at"; // Text
        private const string code19 = "here"; // Text
        private const string code1A = "memItem"; // SURROUNDED BY {   }
        private const string code1B = " for";
        private const string code1C = "memNum..."; // no idea

        public char[] DecodeText(char[] decode, bool symbols)
        {
            ArrayList arrayList = new ArrayList();
            bool lastBrace = true;

            for (int i = 0; i < decode.Length; i++) // For every character of text
            {
                if (symbols) // We are decoding to numbers
                {
                    if (settings.Keystrokes[decode[i]] == "") // Is encoded character
                    {
                        switch ((byte)decode[i]) // Since the byte is encoded, it musts coorespond to one of these cases
                        // All case numbers are for decoding to text for the plain text
                        {
                            case 0x08: AddCharsToArrayList(arrayList, code08.ToCharArray()); break;
                            case 0x09: AddCharsToArrayList(arrayList, code09.ToCharArray()); break;
                            case 0x0A: AddCharsToArrayList(arrayList, code0A.ToCharArray()); break;
                            case 0x0B: // number of spaces
                                if (decode.Length > i + 1)
                                {
                                    i++;
                                    for (int j = 0; j < decode[i]; j++)
                                        arrayList.Add(' ');
                                }
                                break;
                            case 0x0D: goto case 0xFF;
                            case 0x0E: AddCharsToArrayList(arrayList, code0E.ToCharArray()); break;
                            case 0x0F: AddCharsToArrayList(arrayList, code0F.ToCharArray()); break;
                            case 0x10: AddCharsToArrayList(arrayList, code10.ToCharArray()); break;
                            case 0x11: AddCharsToArrayList(arrayList, code11.ToCharArray()); break;
                            case 0x12: AddCharsToArrayList(arrayList, code12.ToCharArray()); break;
                            case 0x13: AddCharsToArrayList(arrayList, code13.ToCharArray()); break;
                            case 0x14: AddCharsToArrayList(arrayList, code14.ToCharArray()); break;
                            case 0x15: AddCharsToArrayList(arrayList, code15.ToCharArray()); break;
                            case 0x16: AddCharsToArrayList(arrayList, code16.ToCharArray()); break;
                            case 0x17: AddCharsToArrayList(arrayList, code17.ToCharArray()); break;
                            case 0x18: AddCharsToArrayList(arrayList, code18.ToCharArray()); break;
                            case 0x19: AddCharsToArrayList(arrayList, code19.ToCharArray()); break;
                            case 0x1B: AddCharsToArrayList(arrayList, code1B.ToCharArray()); break;
                            case 0x1C: goto case 0xFF;
                            case 0xFF:
                                string tem = ((byte)decode[i]).ToString();

                                char[] dec = tem.ToCharArray();
                                arrayList.Add('[');
                                for (int z = 0; z < dec.Length; z++)
                                {
                                    arrayList.Add(dec[z]);
                                }
                                arrayList.Add(']');
                                if (decode.Length > i + 1)
                                {
                                    i++;
                                    goto default;
                                }
                                break;
                            default:
                                string temp = ((byte)decode[i]).ToString();

                                char[] decoded = temp.ToCharArray();
                                arrayList.Add('[');
                                for (int z = 0; z < decoded.Length; z++)
                                {
                                    arrayList.Add(decoded[z]);
                                }
                                arrayList.Add(']');
                                break;

                        }
                    }
                    else // Not encoded character
                        arrayList.Add(Convert.ToChar(settings.Keystrokes[decode[i]]));
                }
                else // We are decoding to words
                {
                    if (settings.Keystrokes[decode[i]] == "") // Current byte is encoded
                    {
                        lastBrace = true;
                        switch ((byte)decode[i])
                        {
                            case 0x00:
                                arrayList.Add('[');
                                AddCharsToArrayList(arrayList, code00.ToCharArray());
                                break;
                            case 0x01:
                                arrayList.Add('[');
                                AddCharsToArrayList(arrayList, code01.ToCharArray());
                                break;
                            case 0x02:
                                arrayList.Add('[');
                                AddCharsToArrayList(arrayList, code02.ToCharArray());
                                break;
                            case 0x03:
                                arrayList.Add('[');
                                AddCharsToArrayList(arrayList, code03.ToCharArray());
                                break;
                            case 0x04:
                                arrayList.Add('[');
                                AddCharsToArrayList(arrayList, code04.ToCharArray());
                                break;
                            case 0x05:
                                arrayList.Add('[');
                                AddCharsToArrayList(arrayList, code05.ToCharArray());
                                break;
                            case 0x06:
                                arrayList.Add('[');
                                AddCharsToArrayList(arrayList, code06.ToCharArray());
                                break;
                            case 0x07:
                                arrayList.Add('[');
                                AddCharsToArrayList(arrayList, code07.ToCharArray());
                                break;
                            case 0x08:
                                AddCharsToArrayList(arrayList, code08.ToCharArray());
                                lastBrace = false;
                                break;
                            case 0x09:
                                AddCharsToArrayList(arrayList, code09.ToCharArray());
                                lastBrace = false;
                                break;
                            case 0x0A:
                                AddCharsToArrayList(arrayList, code0A.ToCharArray());
                                lastBrace = false;
                                break;
                            case 0x0B:
                                i++;
                                for (int j = 0; j < decode[i]; j++)
                                {
                                    arrayList.Add(' ');
                                }
                                lastBrace = false;
                                break;
                            case 0x0C:
                                arrayList.Add('[');
                                AddCharsToArrayList(arrayList, code0C.ToCharArray());
                                break;
                            case 0x0D:
                                arrayList.Add('[');
                                AddCharsToArrayList(arrayList, code0D.ToCharArray());
                                arrayList.Add(']');
                                arrayList.Add('[');
                                arrayList.Add(' ');
                                i++;
                                string tem = ((byte)decode[i]).ToString();

                                char[] dec = tem.ToCharArray();
                                for (int z = 0; z < dec.Length; z++)
                                {
                                    arrayList.Add(dec[z]);
                                }
                                break;
                            case 0x0E:
                                AddCharsToArrayList(arrayList, code0E.ToCharArray());
                                lastBrace = false;
                                break;
                            case 0x0F:
                                AddCharsToArrayList(arrayList, code0F.ToCharArray());
                                lastBrace = false;
                                break;
                            case 0x10:
                                AddCharsToArrayList(arrayList, code10.ToCharArray());
                                lastBrace = false;
                                break;
                            case 0x11:
                                AddCharsToArrayList(arrayList, code11.ToCharArray());
                                lastBrace = false;
                                break;
                            case 0x12:
                                AddCharsToArrayList(arrayList, code12.ToCharArray());
                                lastBrace = false;
                                break;
                            case 0x13:
                                AddCharsToArrayList(arrayList, code13.ToCharArray());
                                lastBrace = false;
                                break;
                            case 0x14:
                                AddCharsToArrayList(arrayList, code14.ToCharArray());
                                lastBrace = false;
                                break;
                            case 0x15:
                                AddCharsToArrayList(arrayList, code15.ToCharArray());
                                lastBrace = false;
                                break;
                            case 0x16:
                                AddCharsToArrayList(arrayList, code16.ToCharArray());
                                lastBrace = false;
                                break;
                            case 0x17:
                                AddCharsToArrayList(arrayList, code17.ToCharArray());
                                lastBrace = false;
                                break;
                            case 0x18:
                                AddCharsToArrayList(arrayList, code18.ToCharArray());
                                lastBrace = false;
                                break;
                            case 0x19:
                                AddCharsToArrayList(arrayList, code19.ToCharArray());
                                lastBrace = false;
                                break;
                            case 0x1A:
                                arrayList.Add('[');
                                AddCharsToArrayList(arrayList, code1A.ToCharArray());
                                break;
                            case 0x1B:
                                AddCharsToArrayList(arrayList, code1B.ToCharArray());
                                lastBrace = false;
                                break;
                            case 0x1C:
                                arrayList.Add('[');
                                AddCharsToArrayList(arrayList, code1C.ToCharArray());
                                arrayList.Add(']');
                                arrayList.Add('[');
                                arrayList.Add(' ');
                                i++;
                                string te = ((byte)decode[i]).ToString();

                                char[] de = te.ToCharArray();
                                for (int z = 0; z < de.Length; z++)
                                {
                                    arrayList.Add(de[z]);
                                }
                                break;
                            default:
                                arrayList.Add('[');
                                AddCharsToArrayList(arrayList, "THIS IS A BUG: ".ToCharArray());
                                string temp = ((byte)decode[i]).ToString();
                                AddCharsToArrayList(arrayList, temp.ToCharArray());
                                break;
                        }
                        if (lastBrace)
                            arrayList.Add(']');
                    }
                    else
                        arrayList.Add(Convert.ToChar(settings.Keystrokes[decode[i]]));
                }
            }

            char[] decodedStr = new char[arrayList.Count];
            arrayList.CopyTo(decodedStr);

            return decodedStr;
        }
        public char[] EncodeText(char[] array, bool symbols)
        {
            bool openQuote = true;
            ArrayList arrayList = new ArrayList();
            char[] backup = array;

            for (int i = 0; i < array.Length; i++)
            {
                if (symbols)
                {
                    if (array[i] == '[' ||
                        array[i] == '\x20' ||
                        array[i] == '\x22')
                    {
                        switch (array[i])
                        {
                            case '[':// Encode {123} to bytes
                                // Can get rid of by using 2 digit hex characters
                                if (array[i + 1] != ']') // would make 1
                                {
                                    char digitOne = (char)(array[i + 1] - 0x30);

                                    if (array.Length > i + 2 && array[i + 2] != ']') // would make 2 digits
                                    {
                                        char digitTwo = (char)(array[i + 2] - 0x30);

                                        if (array.Length > i + 3 && array[i + 3] != ']') // would make 3 digits
                                        {
                                            char digitThree = (char)(array[i + 3] - 0x30);
                                            arrayList.Add((char)((digitOne * 100) + (digitTwo * 10) + digitThree));
                                            i += 4;
                                            break;
                                        }
                                        else // 2 digits
                                        {
                                            arrayList.Add((char)((digitOne * 10) + digitTwo));
                                            i += 3;
                                            break;
                                        }
                                    }
                                    arrayList.Add((char)(digitOne));
                                    i += 2;
                                    break;
                                }
                                break; // none
                            case '\x20':
                                if (SearchForSubstring(array, i, "     ")) // code 0B
                                {
                                    int z = 0;
                                    while (i + z < array.Length && array[i + z] == ' ') z++; //; count spaces
                                    arrayList.Add('\x0B'); // space encode byte
                                    arrayList.Add((char)z); // space count
                                    i += z - 1; // allign array
                                    break;
                                }
                                if (SearchForSubstring(array, i, code0A))
                                {
                                    arrayList.Add('\x0A');
                                    i += code0A.Length - 1;
                                    break;
                                }
                                if (SearchForSubstring(array, i, code09))
                                {
                                    arrayList.Add('\x09');
                                    i += code09.Length - 1;
                                    break;
                                }
                                if (SearchForSubstring(array, i, code08))
                                {
                                    arrayList.Add('\x08');
                                    i += code08.Length - 1;
                                    break;
                                }
                                if (SearchForSubstring(array, i, code0E))
                                {
                                    arrayList.Add('\x0E');
                                    i += code0E.Length - 1;
                                    break;
                                }
                                if (SearchForSubstring(array, i, code0F))
                                {
                                    arrayList.Add('\x0F');
                                    i += code0F.Length - 1;
                                    break;
                                }
                                if (SearchForSubstring(array, i, code11))
                                {
                                    arrayList.Add('\x11');
                                    i += code11.Length - 1;
                                    break;
                                }
                                if (SearchForSubstring(array, i, code14))
                                {
                                    arrayList.Add('\x14');
                                    i += code14.Length - 1;
                                    break;
                                }
                                if (SearchForSubstring(array, i, code15))
                                {
                                    arrayList.Add('\x15');
                                    i += code15.Length - 1;
                                    break;
                                }
                                if (SearchForSubstring(array, i, code17))
                                {
                                    arrayList.Add('\x17');
                                    i += code17.Length - 1;
                                    break;
                                }
                                if (SearchForSubstring(array, i, code1B))
                                {
                                    arrayList.Add('\x1B');
                                    i += code1B.Length - 1;
                                    break;
                                }
                                arrayList.Add('\x20');
                                break;
                            case '\x22':
                                if (openQuote)
                                {
                                    arrayList.Add('\x22');
                                    openQuote = false;
                                }
                                else
                                {
                                    arrayList.Add('\x23');
                                    openQuote = true;
                                }
                                break;
                            default: arrayList.Add('\x20'); break;
                        }
                    }
                    else
                    {
                        switch (array[i])
                        {
                            case 'i':
                                if (SearchForSubstring(array, i, code10))
                                {
                                    arrayList.Add('\x10');
                                    i += code10.Length - 1;
                                    break;
                                }
                                if (SearchForSubstring(array, i, code16))
                                {
                                    arrayList.Add('\x16');
                                    i += code16.Length - 1;
                                    break;
                                }
                                goto default;
                            case '\x27':
                                if (SearchForSubstring(array, i, code12))
                                {
                                    arrayList.Add('\x12');
                                    i += code12.Length - 1;
                                    break;
                                }
                                goto default;
                            case 'M':
                                if (SearchForSubstring(array, i, code13))
                                {
                                    arrayList.Add('\x13');
                                    i += code13.Length - 1;
                                    break;
                                }
                                goto default;
                            case 'h':
                                if (SearchForSubstring(array, i, code19))
                                {
                                    arrayList.Add('\x19');
                                    i += code19.Length - 1;
                                    break;
                                }
                                goto default;
                            case 'a':
                                if (SearchForSubstring(array, i, code18))
                                {
                                    arrayList.Add('\x18');
                                    i += code18.Length - 1;
                                    break;
                                }
                                goto default;
                            default:
                                arrayList.Add(StringIndex(settings.Keystrokes, array[i]));
                                break;
                        }
                    }
                }
                else //!symbols
                {
                    if (array[i] == '[' ||
                        array[i] == '\x20' ||
                        array[i] == '\x22')
                    {
                        switch (array[i])
                        {
                            case '[':
                                int len;
                                i++;
                                for (len = 0; len < array.Length - i && array[i + len] != ']'; len++) ;
                                char[] codeChar = new char[len];
                                for (int z = 0; z < len; z++)
                                    codeChar[z] = array[i + z];
                                string codeStr = new string(codeChar);

                                switch (codeStr)
                                {
                                    case code00: arrayList.Add('\x00'); break;
                                    case code01: arrayList.Add('\x01'); break;
                                    case code02: arrayList.Add('\x02'); break;
                                    case code03: arrayList.Add('\x03'); break;
                                    case code04: arrayList.Add('\x04'); break;
                                    case code05: arrayList.Add('\x05'); break;
                                    case code06: arrayList.Add('\x06'); break;
                                    case code07: arrayList.Add('\x07'); break;
                                    case code0C: arrayList.Add('\x0C'); break;
                                    case code0D:
                                        arrayList.Add('\x0D');
                                        i += len + 1;

                                        if (array[i + 1] == ' ')
                                            i++; // take care of space
                                        if (array[i + 1] != ']') // would make 1
                                        {
                                            char digitOne = (char)(array[i + 1] - 0x30);

                                            if (array.Length > i + 2 && array[i + 2] != ']') // would make 2 digits
                                            {
                                                char digitTwo = (char)(array[i + 2] - 0x30);

                                                if (array.Length > i + 3 && array[i + 3] != ']') // would make 3 digits
                                                {
                                                    char digitThree = (char)(array[i + 3] - 0x30);
                                                    arrayList.Add((char)((digitOne * 100) + (digitTwo * 10) + digitThree));
                                                    len = 4;
                                                    break;
                                                }
                                                else // 2 digits
                                                {
                                                    arrayList.Add((char)((digitOne * 10) + digitTwo));
                                                    len = 3;
                                                    break;
                                                }
                                            }
                                            arrayList.Add((char)(digitOne));
                                            len = 2;
                                            break;
                                        }
                                        break;
                                    case code1A: arrayList.Add('\x1A'); break;
                                    case code1C:
                                        arrayList.Add('\x1C');
                                        i += len + 1;

                                        if (array[i + 1] == ' ')
                                            i++; // take care of space

                                        if (array[i + 1] != ']') // would make 1
                                        {
                                            char digitOne = (char)(array[i + 1] - 0x30);

                                            if (array.Length > i + 2 && array[i + 2] != ']') // would make 2 digits
                                            {
                                                char digitTwo = (char)(array[i + 2] - 0x30);

                                                if (array.Length > i + 3 && array[i + 3] != ']') // would make 3 digits
                                                {
                                                    char digitThree = (char)(array[i + 3] - 0x30);
                                                    arrayList.Add((char)((digitOne * 100) + (digitTwo * 10) + digitThree));
                                                    len = 4;
                                                    break;
                                                }
                                                else // 2 digits
                                                {
                                                    arrayList.Add((char)((digitOne * 10) + digitTwo));
                                                    len = 3;
                                                    break;
                                                }
                                            }
                                            arrayList.Add((char)(digitOne));
                                            len = 2;
                                            break;
                                        }
                                        break;
                                    default: break;
                                }
                                i += len;
                                break;
                            case '\x20':
                                if (SearchForSubstring(array, i, "     ")) // code 0B
                                {
                                    int z = 0;
                                    while (i + z < array.Length && array[i + z] == ' ') z++;
                                    arrayList.Add('\x0B');
                                    arrayList.Add((char)z);
                                    i += z - 1;
                                    break;
                                }
                                if (SearchForSubstring(array, i, code0A))
                                {
                                    arrayList.Add('\x0A');
                                    i += code0A.Length - 1;
                                    break;
                                }
                                if (SearchForSubstring(array, i, code09))
                                {
                                    arrayList.Add('\x09');
                                    i += code09.Length - 1;
                                    break;
                                }
                                if (SearchForSubstring(array, i, code08))
                                {
                                    arrayList.Add('\x08');
                                    i += code08.Length - 1;
                                    break;
                                }
                                if (SearchForSubstring(array, i, code0E))
                                {
                                    arrayList.Add('\x0E');
                                    i += code0E.Length - 1;
                                    break;
                                }
                                if (SearchForSubstring(array, i, code0F))
                                {
                                    arrayList.Add('\x0F');
                                    i += code0F.Length - 1;
                                    break;
                                }
                                if (SearchForSubstring(array, i, code11))
                                {
                                    arrayList.Add('\x11');
                                    i += code11.Length - 1;
                                    break;
                                }
                                if (SearchForSubstring(array, i, code14))
                                {
                                    arrayList.Add('\x14');
                                    i += code14.Length - 1;
                                    break;
                                }
                                if (SearchForSubstring(array, i, code15))
                                {
                                    arrayList.Add('\x15');
                                    i += code15.Length - 1;
                                    break;
                                }
                                if (SearchForSubstring(array, i, code17))
                                {
                                    arrayList.Add('\x17');
                                    i += code17.Length - 1;
                                    break;
                                }
                                if (SearchForSubstring(array, i, code1B))
                                {
                                    arrayList.Add('\x1B');
                                    i += code1B.Length - 1;
                                    break;
                                }
                                arrayList.Add('\x20');
                                break;
                            case '\x22': // handles user input quotes
                                if (openQuote)
                                {
                                    arrayList.Add('\x22');
                                    openQuote = false;
                                }
                                else
                                {
                                    arrayList.Add('\x23');
                                    openQuote = true;
                                }
                                break;
                            default: arrayList.Add('\x20'); break;
                        }
                    }
                    else
                    {
                        switch (array[i])
                        {
                            case 'i':
                                if (SearchForSubstring(array, i, code10))
                                {
                                    arrayList.Add('\x10');
                                    i += code10.Length - 1;
                                    break;
                                }
                                if (SearchForSubstring(array, i, code16))
                                {
                                    arrayList.Add('\x16');
                                    i += code16.Length - 1;
                                    break;
                                }
                                goto default;
                            case '\x27':
                                if (SearchForSubstring(array, i, code12))
                                {
                                    arrayList.Add('\x12');
                                    i += code12.Length - 1;
                                    break;
                                }
                                goto default;
                            case 'M':
                                if (SearchForSubstring(array, i, code13))
                                {
                                    arrayList.Add('\x13');
                                    i += code13.Length - 1;
                                    break;
                                }
                                goto default;
                            case 'h':
                                if (SearchForSubstring(array, i, code19))
                                {
                                    arrayList.Add('\x19');
                                    i += code19.Length - 1;
                                    break;
                                }
                                goto default;
                            case 'a':
                                if (SearchForSubstring(array, i, code18))
                                {
                                    arrayList.Add('\x18');
                                    i += code18.Length - 1;
                                    break;
                                }
                                goto default;
                            default:
                                arrayList.Add(StringIndex(settings.Keystrokes, array[i]));
                                break;
                        }
                    }
                }
            }

            char[] encodedStr = new char[arrayList.Count];
            try
            {
                arrayList.CopyTo(encodedStr);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Input Error, text not valid. You probably entered a character that Super Mario RPG cannot parse. This text will not be saved unless the error is fixed.");
                error = true;
                return backup;
            }

            if (!VerifyText(encodedStr))
            {
                //MessageBox.Show("The text input is invalid. Please verify '[' and ']' brackets.\n\nIt may also be a character in the text that the ROM cannot parse.\nThis text will not be saved unless the error is fixed.", "LAZY SHELL");
                error = true;
                return backup;
            }
            error = false;


            return encodedStr;
        }
        public void AddCharsToArrayList(ArrayList arrayList, char[] chars)
        {
            for (int i = 0; i < chars.Length; i++)
                arrayList.Add(chars[i]);
        }
        public bool SearchForSubstring(char[] array, int index, string substring)
        {
            char[] subStr = substring.ToCharArray();
            if (array.Length < substring.Length + index)
                return false;
            for (int i = 0; i < subStr.Length; i++)
            {
                if (subStr[i] != array[i + index])
                    return false;
            }
            return true;
        }
        public char StringIndex(StringCollection strings, char character)
        {
            for (char i = (char)0; i < strings.Count; i++)
            {
                if (strings[i] == character.ToString())
                    return i;
            }
            return character;
        }
        public bool IsValidChar(char toTest)
        {
            if (toTest >= '\x00' && toTest <= '\x1C') return true;
            if (toTest >= '\x20' && toTest <= '\x5A') return true;
            if (toTest >= '\x5C' && toTest <= '\x9F') return true;
            foreach (string temp in settings.Keystrokes)
                if (temp != "" && Convert.ToChar(temp) == toTest) return true;
            return false;
        }
        public bool VerifyText(char[] toTest)
        {
            bool openBracket = false;
            if (toTest.Length == 0)
                return true;
            for (int i = 0; i < toTest.Length; i++)
            {
                if (toTest[i] != '[' && toTest[i] != ']' && IsValidChar(toTest[i]) == false)
                    if (i == 0 || (toTest[i - 1] != '\x0B' && toTest[i - 1] != '\x0D'))
                        return false;

                if (toTest[i] == '[')
                {
                    if (openBracket == true)
                        return false;
                    openBracket = true;
                }
                if (toTest[i] == ']')
                {
                    if (openBracket == false)
                        return false;
                    openBracket = false;
                }
            }
            if (openBracket == false)
                return true;
            return false;
        }
        public bool VerifyCorrectSymbols(char[] toVerify, bool format)
        {
            bool symbol = false, found = false;
            try
            {
                for (int i = 0; i < toVerify.Length; i++)
                {
                    if (toVerify[i] == '[')
                    {
                        if (toVerify[i + 1] >= '\x30' && toVerify[i + 1] <= '\x39')
                            symbol = true;
                        else
                            symbol = false;
                        found = true;
                    }
                    if (format != symbol && found)
                        return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}