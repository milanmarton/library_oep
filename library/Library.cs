using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

namespace library
{
    public class Library
    {
        private static Library instance = null;
        private List<Member> members;
        private List<Book> books;

        private Library()
        {
            members = new List<Member>();
            books = new List<Book>();
        }

        public static Library Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Library();
                }
                return instance;
            }
        }

        public void AddMember(Member member)
        {
            members.Add(member);
            Console.WriteLine($"{member.getName()} has become a member.");
        }

        public void AddBook(Book book) // ide meg kell a unique miatt checkolni az ID jat a booknak
        {
            if (books.Any(b => b.getID() == book.getID()))
            {
                throw new InvalidOperationException($"A book with the same ID ({book.getID()}) already exists in the library.");
            }
            books.Add(book);
            Console.WriteLine($"{book.getTitle()} is added to the library.");
        }

        public void BorrowBook(Member member, int bookID, DateTime dueDate)
        {
            Book book = FindBookByID(bookID);
            if (book == null)
            {
                throw new Exception($"Book with ID {bookID} not found in the library.");
            }
            book.Borrow(member, dueDate);
            Console.WriteLine($"{book.getTitle()} just got borrowed by {member.getName()} with the due date of {dueDate}.");
        }

        public void ReturnBook(Member member, int bookID)
        {
            Book book = FindBookByID(bookID);
            if (book == null)
            {
                throw new Exception($"Book with ID {bookID} not found in the library.");
            }
            book.Return(member);
            Console.WriteLine($"{book.getTitle()} just got returned by {member.getName()}.");
        }

        public decimal CalculateLateFee(Member member)
        {
            decimal lateFee = 0;
            var borrowedBookIDs = member.GetBorrowedBookIDs();

            foreach (int bookID in borrowedBookIDs)
            {
                Book book = FindBookByID(bookID);
                int daysOverdue = (DateTime.Now - book.getDueDate()).Days;
                if (daysOverdue > 0)
                {
                    decimal bookLateFee = daysOverdue * book.Fee();
                    lateFee += bookLateFee;
                }
            }


            return lateFee;
        }

        private Book FindBookByID(int bookID)
        {
            Book book = books.Find(book => book.getID() == bookID);
            if (book == null)
            {
                throw new Exception($"Book with ID {bookID} not found in the library.");
            }
            return book;
        }

        public Member FindMemberByName(string memberName)
        {
            Member member = members.Find(m => m.getName() == memberName);
            if (member == null)
            {
                throw new Exception($"Member with name {memberName} not found in the library.");
            }
            return member;
        }
    }
}
