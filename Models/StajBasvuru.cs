namespace StajyerTakip.Models
{
    public class StajBasvuru
    {
        public int Id { get; set; }
        public string LocationOfBasvuruFormu { get; set; }
        public string LocationOfStajSozlesmesi { get; set; }

        public StajBasvuru(int ıd, string locationOfBasvuruFormu, string locationOfStajSozlesmesi)
        {
            Id = ıd;
            LocationOfBasvuruFormu = locationOfBasvuruFormu;
            LocationOfStajSozlesmesi = locationOfStajSozlesmesi;
        }

        public StajBasvuru()
        {
            Id = 0;
            LocationOfBasvuruFormu = "";
            LocationOfStajSozlesmesi = "";
        }
    }
}
