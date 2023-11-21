namespace CGMD.Models{
    public class Piece {

        public int ID { get; set; }
        public string Composer { get; set; } = string.Empty;
        public string? YOB { get; set; }
        public string? YOD { get; set; }
        public string? Nation { get; set; }
        public string Work { get; set; } = string.Empty; public string? Tags { get; set; }
        public string? Inst { get; set; }
        public string? keyOf { get; set; }
        public string? Video { get; set; }
        public string? Score { get; set; }

        //public string? Email { get; set; }

        public Piece(){

        }
    }
}
