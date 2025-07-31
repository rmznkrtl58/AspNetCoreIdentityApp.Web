namespace AspNetCoreIdentityApp.Core.ViewModels
{
    public class ClaimViewModel
    {
        //Third Pary uygulamalardan giriş yaptığımızda ordan gelen claimlerde olacak kimden geldiğini öğrenmek için Issuer Kısmınıda dahil ettim.
        public string Issuer { get; set; }
        public string Type { get; set; } = default!;
        public string Value { get; set; } = default!;
        //Type(Key)-Value Doğum Tarihi-04.11.2003
        //örnek senaryo borsa sitesi 18 yaşından küçüklere ilgili sayfayı gösterme 18 yaşından büyüklere göster.
    }
}
