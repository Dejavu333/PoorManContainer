using PoorManContainer;

namespace Program
{
    class Program
    {
        public static void Main(string[] args)
        {
            Cont.New<IVmi2>("Very Well");
        }

    }
        public interface IVmi2
        {
        }

        public class ImplVmi2:IVmi2
        {
            public ImplVmi2(string s)
            {
                Console.WriteLine(s);
            }
        }
}