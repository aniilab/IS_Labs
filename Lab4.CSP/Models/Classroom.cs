namespace Lab4.CSP.Models
{
    public class Classroom
    {
        public int Room { get; }
        public bool IsBig { get; }

        public Classroom(int room, bool isBig)
        {
            Room = room;
            IsBig = isBig;
        }

        public override string ToString() => $"Classroom #{Room} ({(IsBig ? "big" : "small")})";
    }
}
