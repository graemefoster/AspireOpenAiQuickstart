using System.ComponentModel;

namespace OpenAiQuickstart.BusinessApi.Queries;

public enum Category
{
    [Description("Restaturants")]
    Restaurant = 0,
    [Description("Exercise e.g. Gymnasium, Personal Trainer, etc.")]
    Exercise = 1,
    [Description("Food e.g. Groceries, Supermarket, etc.")]
    Food = 2,
    [Description("Car e.g. Petrol, Car Wash, etc.")]
    Car = 3,
    [Description("Entertainment, e.g. Movies, Nightclubs etc.")]
    Entertainment = 4
}