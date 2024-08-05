using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library
{
    public enum Genre
    {
        Nature,
        Fiction,
        Juvenile
    }

    public abstract class Book
    {
        private string Title;
        private string Author;
        private string Publisher;
        private Int64 ISBN;
        private int PageCount;
        private int ID;
        private Genre Genre;
        private IRarityBehavior Rarity;
        private bool Available;
        private DateTime DueDate;

        public Book(string t, string au, string pub, Int64 i, int pc, int id, Genre g, IRarityBehavior r, bool a)
        {
            this.Title = t; this.Author = au; this.Publisher = pub; this.ISBN = i; this.PageCount = pc; this.ID = id; this.Genre = g; this.Rarity = r; this.Available = a; this.DueDate = new DateTime();
        }

        public int getID()
        {
            return ID;
        }
        public bool getAvaliable()
        {
            return Available;
        }

        public void setAvaliable(bool value)
        {
            Available = value;
        }
        public IRarityBehavior GetRarityBehavior() { return Rarity; }

        public string getTitle() { return Title; }
        public DateTime getDueDate() { return DueDate; }
        public void setDueDate(DateTime dueDate) { DueDate = dueDate; }

        public void Borrow(Member member,DateTime dueDate)
        {
            if (!Available)
            {
                throw new InvalidOperationException("Book is not available for borrowing.");
            }

            Available = false;
            DueDate = dueDate;

            member.BorrowBook(ID);
        }

        public void Return(Member member)
        {
            Available = true;

            member.ReturnBook(ID);
        }

        public abstract decimal Fee();

        public static Book FromTxt(string txtLine)
        {
            string[] fields = txtLine.Split(',');

            Genre genre = (Genre)Enum.Parse(typeof(Genre), fields[6]);
            //Rarity rarity = (Rarity)Enum.Parse(typeof(Rarity), fields[7]);
            IRarityBehavior rarity;
            string rar = fields[7];

            switch (rar)
            {
                case "Rare":
                    rarity = RareBehavior.Instance;
                    break;
                case "NotRare":
                    rarity = NotRareBehavior.Instance;
                    break;
                default:
                    throw new ArgumentException($"Unknown rarity: {rar}");
            }

            string title = fields[0];
            string author = fields[1];
            string publisher = fields[2];
            Int64 isbn = Int64.Parse(fields[3]);
            int pageCount = int.Parse(fields[4]);
            int id = int.Parse(fields[5]);
            bool av = string.IsNullOrEmpty(fields[8]) ? false : bool.Parse(fields[8]);

            Book book;

            switch (genre)
            {
                case Genre.Nature:
                    book = new NatureBook(title, author, publisher, isbn, pageCount, id, genre, rarity, av);
                    break;
                case Genre.Fiction:
                    book = new FictionBook(title, author, publisher, isbn, pageCount, id, genre, rarity, av);
                    break;
                case Genre.Juvenile:
                    book = new JuvenileBook(title, author, publisher, isbn, pageCount, id, genre, rarity, av);
                    break;
                default:
                    throw new ArgumentException($"Unknown genre: {genre}");
            }

            return book;
        }

        public class NatureBook : Book
        {
            public NatureBook(string t, string au, string pub, Int64 i, int pc, int id, Genre g, IRarityBehavior r, bool a) : base(t, au, pub, i, pc, id, g, r, a) { }
            public override decimal Fee()
            {
                return Rarity.CalculateLateFee(this);
            }
        }

        public class FictionBook : Book
        {
            public FictionBook(string t, string au, string pub, Int64 i, int pc, int id, Genre g, IRarityBehavior r, bool a) : base(t, au, pub, i, pc, id, g, r, a) { }
            public override decimal Fee()
            {
                return Rarity.CalculateLateFee(this);
            }
        }

        public class JuvenileBook : Book
        {
            public JuvenileBook(string t, string au, string pub, Int64 i, int pc, int id, Genre g, IRarityBehavior r, bool a) : base(t, au, pub, i, pc, id, g, r, a) { }
            public override decimal Fee()
            {
                return Rarity.CalculateLateFee(this);
            }
        }


    }
}
