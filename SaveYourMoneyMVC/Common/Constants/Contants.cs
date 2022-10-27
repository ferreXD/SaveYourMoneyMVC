namespace SaveYourMoneyMVC.Common.Constants
{
    public static class Contants
    {
        public static readonly string LANGUAGE_ES = "es-ES";
        public static readonly string LANGUAGE_US = "en-US";
        public static readonly string LANGUAGE_EN = "en-EN";

        public static List<string> LANGUAGES = new List<string>() { LANGUAGE_ES, LANGUAGE_EN, LANGUAGE_US };

        public static List<string> EXPENDS_TYPE_EN = new List<string>()
        {
            "ELECTRICITY",
            "FUEL",
            "COMMUNICATION",
            "FOOD",
            "LEISURE",
            "RENT",
            "STUDIES",
            "CLOTHING",
            "SUBSCRIPTIONS",
            "HOLIDAYS",
            "PRIVATE",
            "PUBLIC TRANSPORT"
        };

        public static List<string> EXPENDS_TYPE_ES = new List<string>()
        {
            "LUZ",
            "COMBUSTIBLE",
            "COMUNICACIONES",
            "ALIMENTACION",
            "OCIO",
            "ALQUILER",
            "ESTUDIOS",
            "ROPA",
            "SUBSCRIPCIONES",
            "VACACIONES",
            "PRIVADO",
            "TRANSPORTE PÚBLIC"
        };

        public static List<string> MONTHS_ES = new List<string>()
        {
            "ENERO",
            "FEBRERO",
            "MARZO",
            "ABRIÑ",
            "MAYO",
            "JUNIO",
            "JULIO",
            "AGOSTO",
            "SEPTIEMBRE",
            "OCTUBRE",
            "NOVIEMBRE",
            "DICIEMBRE",
        };

        public static List<string> MONTHS_EN = new List<string>()
        {
            "JANUARY",
            "FEBRUARY",
            "MARCH",
            "APRIL",
            "MAY",
            "JUNE",
            "JULY",
            "AUGUST",
            "SEPTEMBER",
            "OCTOBER",
            "NOVEMBER",
            "DECEMBER",
        };
    }
}
