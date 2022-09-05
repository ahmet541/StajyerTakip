using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StajyerTakip.Models
{
    public enum Status
    {
        başlamadı,
        devam_ediyor,
        bitirdi,
        ihlal
    }
    public class StajyerModel
    {


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]

        public int Id { get; set; }
        [Required(AllowEmptyStrings = false,ErrorMessage = " Bu alan boş bırakılamaz")]
        [Display(Name = "Stajer'in Adı ve Soyadı")]
        public string Name { get; set; }

        
        [StringLength(11, MinimumLength =11, ErrorMessage = "T.C. Kimlik No 11 Rakamdan Oluşmalı.")]
        [Required(ErrorMessage="T.C. Kimlik No girilmesi gerekmektedir.")]
        [Display(Name = "T.C. Kimlik No")]
        public string IdentificationNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Başlangıç Tarihi")]
        [Required(ErrorMessage = "Başlangıç Tarihi girilmesi gerekmektedir.")]
        public DateTime StartingDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Bitiş Tarihi")]
        [Required(ErrorMessage = "T.C. Kimlik No girilmesi gerekmektedir.")]
        public DateTime EndingDate { get; set; }

        [ScaffoldColumn(false)]
        public string LocationOfCV { get; set; }

        [ScaffoldColumn(false)]
        public string LocationOfPicture { get; set; }

        [UIHint("StatusMenu")]
        [Display(Name = "Staj Durumu")]
        public Status State { get; set; }

        public StajyerModel(string name, string ıdentificationNumber, DateTime startingDate, DateTime endingDate, string locationOfCV, string locationOfImg)
        {
            Id = 0;
            Name = name;
            IdentificationNumber = ıdentificationNumber;
            StartingDate = startingDate;
            EndingDate = endingDate;
            LocationOfCV = locationOfCV;
            LocationOfPicture = locationOfImg;
            State = Status.başlamadı;
        }
        public StajyerModel(string name, string ıdentificationNumber, DateTime startingDate, DateTime endingDate)
        {
            Id = 0;
            Name = name;
            IdentificationNumber = ıdentificationNumber;
            StartingDate = startingDate;
            EndingDate = endingDate;
            LocationOfCV = "";
            LocationOfPicture = "defaultPictureForStajyer.jpg";
            State = Status.başlamadı;
        }

        public StajyerModel()
        {
            Id = 0;
            Name = "";
            IdentificationNumber = "";
            StartingDate = new DateTime(DateTime.Now.Year, 1,1);
            EndingDate = new DateTime(DateTime.Now.Year, 1, 1);
            LocationOfCV = "";
            LocationOfPicture = "defaultPictureForStajyer.jpg";
            State = Status.başlamadı;
        }

        public string GetDate()
        {
            return "date";
        } 

        public bool isValid()
        {
            return (Name!=null) && (IdentificationNumber != null) && (StartingDate != null) && (EndingDate != null);
        }
    }

}
