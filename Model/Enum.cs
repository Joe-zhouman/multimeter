// Joe
// 周漫
// 2020110216:31

namespace Model {
    public enum TestMethod {
        KAPPA = 1,
        ITC,
        ITM,
        ITMS
    }

    public enum UserType {
        NORMAL,
        ADVANCE
    }

    public enum SpecimenType {
        HEAT_METER,
        SAMPLE
    }

    public enum ProbeType {
        NULL,
        CUBIC_POLY_VOLTAGE,
        THERMOCOUPLE,
        THERMISTOR,
        CUBIC_POLY_RESISTANCE
    }
    public enum PromptType {
        INFO,
        WARNING,
        ERROR
    }
}