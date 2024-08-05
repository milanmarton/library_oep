using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static library.Book;

namespace library
{
    public interface IRarityBehavior
    {
        decimal CalculateLateFee(NatureBook book);
        decimal CalculateLateFee(FictionBook book);
        decimal CalculateLateFee(JuvenileBook book);
    }

    public class RareBehavior : IRarityBehavior
    {
        private static RareBehavior instance = null;
        public decimal CalculateLateFee(NatureBook book) { return 100; }
        public decimal CalculateLateFee(FictionBook book) { return 50; }
        public decimal CalculateLateFee(JuvenileBook book) { return 30; }
        private RareBehavior() { }

        public static RareBehavior Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RareBehavior();
                }
                return instance;
            }
        }
    }

    public class NotRareBehavior : IRarityBehavior
    {
        private static NotRareBehavior instance = null;
        public decimal CalculateLateFee(NatureBook book) { return 20; }
        public decimal CalculateLateFee(FictionBook book) { return 10; }
        public decimal CalculateLateFee(JuvenileBook book) { return 10; }
        private NotRareBehavior() { }

        public static NotRareBehavior Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new NotRareBehavior();
                }
                return instance;
            }
        }
    }
}
