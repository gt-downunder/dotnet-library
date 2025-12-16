using System;
using System.Linq;
using System.Threading;
using DotNet.Library.Extensions;
using RandomNameGeneratorLibrary;

namespace DotNet.Library.Tests;

public static class RandomFactory
{
    private static readonly Random Random = new();
    private static readonly Lock RandLock = new();
    private static readonly PersonNameGenerator PersonNameGenerator = new(Random);

    // https://en.wikipedia.org/wiki/List_of_fictional_Microsoft_companies
    private static readonly string[] CompanyNames =
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

    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    private static readonly string[] CodeNames =
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

    private static readonly string[] EmailSuffixes =
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

    private static readonly string[] AddressSuffixes =
    [
        "Street",
        "Way",
        "Place",
        "Boulevard",
        "Avenue",
        "Highway"
    ];

    private static readonly string[] Countries =
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
        lock (RandLock)
        {
            return Random.NextInt64(inclusiveMin, exclusiveMax);
        }
    }

    public static int GetInteger(int inclusiveMin = 0, int exclusiveMax = int.MaxValue)
    {
        lock (RandLock)
        {
            return Random.Next(inclusiveMin, exclusiveMax);
        }
    }

    public static string GetCompanyName()
    {
        lock (RandLock)
        {
            return CompanyNames[Random.Next(0, CompanyNames.Length)];
        }
    }

    public static string GetAlphanumericString(int length)
    {
        lock (RandLock)
        {
            return new string([.. Enumerable.Repeat(Chars, length).Select(s => s[Random.Next(s.Length)])]);
        }
    }

    public static bool GetBoolean()
    {
        lock (RandLock)
        {
            return Random.Next(0, 2) == 1;
        }
    }

    public static string GetCountry()
    {
        lock (RandLock)
        {
            return Countries[Random.Next(0, Countries.Length)];
        }
    }

    public static string GetAddressSuffix()
    {
        lock (RandLock)
        {
            return AddressSuffixes[Random.Next(0, AddressSuffixes.Length)];
        }
    }

    public static string GetPersonFullName()
    {
        lock (RandLock)
        {
            return PersonNameGenerator.GenerateRandomFirstAndLastName();
        }
    }

    public static string GetCodeName()
    {
        lock (RandLock)
        {
            return CodeNames[Random.Next(CodeNames.Length)];
        }
    }

    public static string GetEmailAddress(string personName, string companyName)
    {
        var name = personName.RemoveSpecialCharacters().ToLower();
        var company = companyName.RemoveSpecialCharacters().ToLower();

        lock (RandLock)
        {
            var suffix = EmailSuffixes[Random.Next(0, EmailSuffixes.Length)];

            return $"{name}@{company}.{suffix}";
        }
    }

    public static TEnumType GetEnum<TEnumType>() where TEnumType : struct
    {
        lock (RandLock)
        {
            return Enum.Parse<TEnumType>(GetInteger(0, Enum.GetNames(typeof(TEnumType)).Length).ToString());
        }
    }

    public static string[] GetStringArray(int numberOfItems)
    {
        var array = new string[numberOfItems];
        for (var i = 0; i < numberOfItems; i++)
        {
            lock (RandLock)
            {
                array[i] = GetAlphanumericString(20);
            }
        }

        return array;
    }

    public static string GetSemicolonDelimitedString(int numberOfItems)
    {
        lock (RandLock)
        {
            return string.Join(";", GetStringArray(numberOfItems));
        }
    }
}