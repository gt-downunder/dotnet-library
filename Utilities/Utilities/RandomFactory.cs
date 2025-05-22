using DotNet.Library.Extensions;
using RandomNameGeneratorLibrary;
using System;
using System.Linq;
using System.Threading;

namespace DotNet.Library.Utilities
{
    public static class RandomFactory
    {
        private static readonly Random _random = new();
        private static readonly Lock _randLock = new();
        private static readonly PersonNameGenerator _personNameGenerator = new(_random);

        // https://en.wikipedia.org/wiki/List_of_fictional_Microsoft_companies
        private static readonly string[] _companyNames =
        [
            "A. Datum Corporation",
            "AdventureWorks Cycles",
            "Alpine Ski House",
            "Awesome Computers",
            "Baldwin Museum of Science",
            "Blue Yonder Airlines",
            "City Power & Light",
            "Coho Vineyard & Winery",
            "Consolidated Messenger",
            "Contoso Ltd.",
            "cpandl.com",
            "CRONUS",
            "Electronic, Inc.",
            "Fabrikam, Inc.",
            "Fourth Coffee",
            "FusionTomo",
            "Graphic Design Institute",
            "Humongous Insurance",
            "ItExamWorld.com",
            "LitWare Inc.",
            "Lucerne Publishing",
            "Margie's Travel",
            "Northridge Video",
            "Northwind Traders",
            "Olympia",
            "Parnell Aerospace",
            "ProseWare, Inc.",
            "School of Fine Art",
            "Southbridge Video",
            "TailSpin Toys",
            "Tasmanian Traders",
            "The Phone Company",
            "Trey Research Inc.",
            "The Volcano Coffee Company",
            "WingTip Toys",
            "Wide World Importers",
            "Woodgrove Bank"
        ];

        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        private static readonly string[] _codeNames =
        [
            "Algiers",
            "Amazon",
            "Amsterdam",
            "Annapurna",
            "Aphrodite",
            "Apollo",
            "Ares",
            "Artemis",
            "Athens",
            "Baltis",
            "Berlin",
            "Bogota",
            "Calabar",
            "Casablanca",
            "Caspian",
            "Centaurus",
            "Ceres",
            "Demeter",
            "Dresden",
            "Erie",
            "Eris",
            "Everest",
            "Flora",
            "Geneva",
            "Giza",
            "Hades",
            "Halifax",
            "Helsinki",
            "Hestia",
            "Huron",
            "Jakar",
            "Janus",
            "Juno",
            "Jupiter",
            "K2",
            "Kathmandu",
            "Keflavik",
            "Kingston",
            "Kyoto",
            "Ladoga",
            "Luxor",
            "Malawi",
            "Manila",
            "Maribor",
            "Mars",
            "Melbourne",
            "Mercury",
            "Minerva",
            "Mississippi",
            "Nazret",
            "Neptune",
            "Nile",
            "Orcus",
            "Perth",
            "Pomona",
            "Poseidon",
            "Ridder",
            "Rift",
            "Riga",
            "Saimaa",
            "Sarajevo",
            "Sarband",
            "Saturn",
            "Seine",
            "Sol",
            "Sparta",
            "Strand",
            "Tallinn",
            "Tellus",
            "Themes",
            "Toledo",
            "Trevi",
            "Turku",
            "Venus",
            "Vesta",
            "Vilnius",
            "Visby",
            "Vulcan",
            "Westeros",
            "Zeus"
        ];

        private static readonly string[] _emailSuffixes =
        [
            "com",
            "org",
            "net",
            "edu",
            "gov",
            "mil",
            "io",
            "com.uk",
            "com.au",
            "com.vn",
            "com.cn"
        ];

        private static readonly string[] _addressSuffixes =
        [
            "Street",
            "Way",
            "Place",
            "Boulevard",
            "Avenue",
            "Highway"
        ];

        private static readonly string[] _countries =
        [
            "United States",
            "China",
            "Vietnam",
            "Mexico",
            "France",
            "Germany",
            "Canada",
            "Australia"
        ];

        public static long GetLong(long inclusiveMin = 0, long exclusiveMax = long.MaxValue)
        {
            lock (_randLock)
            {
                return _random.NextInt64(inclusiveMin, exclusiveMax);
            }
        }

        public static int GetInteger(int inclusiveMin = 0, int exclusiveMax = int.MaxValue)
        {
            lock (_randLock)
            {
                return _random.Next(inclusiveMin, exclusiveMax);
            }
        }

        public static string GetCompanyName()
        {
            lock (_randLock)
            {
                return _companyNames[_random.Next(0, _companyNames.Length)];
            }
        }

        public static string GetAlphanumericString(int length)
        {
            lock (_randLock)
            {
                return new string([.. Enumerable.Repeat(_chars, length).Select(s => s[_random.Next(s.Length)])]);
            }
        }

        public static bool GetBoolean()
        {
            lock (_randLock)
            {
                return _random.Next(0, 2) == 1;
            }
        }

        public static string GetCountry()
        {
            lock (_randLock)
            {
                return _countries[_random.Next(0, _countries.Length)];
            }
        }

        public static string GetAddressSuffix()
        {
            lock (_randLock)
            {
                return _addressSuffixes[_random.Next(0, _addressSuffixes.Length)];
            }
        }

        public static string GetPersonFullName()
        {
            lock (_randLock)
            {
                return _personNameGenerator.GenerateRandomFirstAndLastName();
            }
        }

        public static string GetCodeName()
        {
            lock (_randLock)
            {
                return _codeNames[_random.Next(_codeNames.Length)];
            }
        }

        public static string GetEmailAddress(string personName, string companyName)
        {
            var name = personName.RemoveSpecialCharacters().ToLower();
            var company = companyName.RemoveSpecialCharacters().ToLower();

            lock (_randLock)
            {
                var suffix = _emailSuffixes[_random.Next(0, _emailSuffixes.Length)];

                return $"{name}@{company}.{suffix}";
            }
        }

        public static TEnumType GetEnum<TEnumType>() where TEnumType : struct
        {
            lock (_randLock)
            {
                return Enum.Parse<TEnumType>(GetInteger(0, Enum.GetNames(typeof(TEnumType)).Length).ToString());
            }
        }

        public static string[] GetStringArray(int numberOfItems)
        {
            string[] array = new string[numberOfItems];
            for (int i = 0; i < numberOfItems; i++)
            {
                lock (_randLock)
                {
                    array[i] = GetAlphanumericString(20);
                }
            }
            return array;
        }

        public static string GetSemicolonDelimitedString(int numberOfItems)
        {
            lock (_randLock)
            {
                return string.Join(";", GetStringArray(numberOfItems));
            }
        }
    }
}
