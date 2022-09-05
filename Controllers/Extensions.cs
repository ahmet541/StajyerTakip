using Microsoft.AspNetCore.Mvc.Rendering;


namespace StajyerTakip.Controllers
{
    public static class Extensions
    {
        public static List<SelectListItem> EnumToSelectList(Type enumType)
        {
            return Enum
              .GetValues(enumType)
              .Cast<int>()
              .Select(i => new SelectListItem
              {
                  Value = i.ToString(),
                  Text = Enum.GetName(enumType, i),
              }
              )
              .ToList();
        }
    }
}
