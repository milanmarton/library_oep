using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using library;

namespace LibraryTests
{
    [TestClass]
    public class LibraryTests
    {
        private Library libraryInstance;
        private Member testMember;

        [TestInitialize]
        public void TestInitialize()
        {
            libraryInstance = Library.Instance;
            testMember = new Member("John Doe");
            libraryInstance.AddMember(testMember);
        }

        [TestMethod]
        public void Test_AddBook()
        {
            var book = new Book.NatureBook("Nature's Wonders", "Author", "Publisher", 1234567890123, 300, 1, Genre.Nature, RareBehavior.Instance, true);
            libraryInstance.AddBook(book);

            Assert.ThrowsException<InvalidOperationException>(() => libraryInstance.AddBook(book));
        }

        [TestMethod]
        public void Test_BorrowBook()
        {
            var book = new Book.FictionBook("Fantastic Voyage", "Author", "Publisher", 2345678901234, 400, 2, Genre.Fiction, NotRareBehavior.Instance, true);
            libraryInstance.AddBook(book);
            libraryInstance.BorrowBook(testMember, 2, DateTime.Now.AddDays(7));
            Assert.IsFalse(book.getAvaliable());
        }

        [TestMethod]
        public void Test_ReturnBook()
        {
            var book = new Book.FictionBook("Fantastic Voyage", "Author", "Publisher", 2345678901234, 400, 3, Genre.Fiction, NotRareBehavior.Instance, true);
            libraryInstance.AddBook(book);
            libraryInstance.BorrowBook(testMember, 3, DateTime.Now.AddDays(7));
            libraryInstance.ReturnBook(testMember, 3);
            Assert.IsTrue(book.getAvaliable());
        }

        [TestMethod]
        public void Test_CalculateLateFee()
        {
            var book = new Book.FictionBook("Fantastic Voyage", "Author", "Publisher", 2345678901234, 400, 4, Genre.Fiction, NotRareBehavior.Instance, true);
            libraryInstance.AddBook(book);
            libraryInstance.BorrowBook(testMember, 4, DateTime.Now.AddDays(-2)); // overdue by 2 days
            decimal lateFee = libraryInstance.CalculateLateFee(testMember);

            Assert.AreEqual(20, lateFee); // 2 days * 10 fee per day for NotRare Fiction Book
        }

        [TestMethod]
        public void Test_RarityBehavior()
        {
            var rareBook = new Book.NatureBook("Nature's Wonders", "Author", "Publisher", 1234567890123, 300, 5, Genre.Nature, RareBehavior.Instance, true);
            var notRareBook = new Book.NatureBook("Nature's Beauty", "Author", "Publisher", 1234567890124, 200, 6, Genre.Nature, NotRareBehavior.Instance, true);

            decimal rareLateFee = RareBehavior.Instance.CalculateLateFee(rareBook);
            decimal notRareLateFee = NotRareBehavior.Instance.CalculateLateFee(notRareBook);

            Assert.AreEqual(100, rareLateFee);
            Assert.AreEqual(20, notRareLateFee);
        }

        [TestMethod]
        public void Test_BorrowUnavailableBook()
        {
            var book = new Book.FictionBook("Fantastic Voyage", "Author", "Publisher", 2345678901234, 400, 7, Genre.Fiction, NotRareBehavior.Instance, false);
            libraryInstance.AddBook(book);

            Assert.ThrowsException<InvalidOperationException>(() => libraryInstance.BorrowBook(testMember, 7, DateTime.Now.AddDays(7)));
        }

        [TestMethod]
        public void Test_ReturnNotBorrowedBook()
        {
            var book = new Book.FictionBook("Fantastic Voyage", "Author", "Publisher", 2345678901234, 400, 8, Genre.Fiction, NotRareBehavior.Instance, true);
            libraryInstance.AddBook(book);

            Assert.ThrowsException<InvalidOperationException>(() => libraryInstance.ReturnBook(testMember, 8));
        }

        [TestMethod]
        public void Test_MemberExceedBorrowLimit()
        {
            for (int i = 0; i < 5; i++)
            {
                var book = new Book.FictionBook($"Book {i}", "Author", "Publisher", 2345678901234, 400, i + 9, Genre.Fiction, NotRareBehavior.Instance, true);
                libraryInstance.AddBook(book);
                libraryInstance.BorrowBook(testMember, i + 9, DateTime.Now.AddDays(7));
            }

            var extraBook = new Book.FictionBook("Extra Book", "Author", "Publisher", 2345678901234, 400, 14, Genre.Fiction, NotRareBehavior.Instance, true);
            libraryInstance.AddBook(extraBook);

            Assert.ThrowsException<InvalidOperationException>(() => libraryInstance.BorrowBook(testMember, 14, DateTime.Now.AddDays(7)));
        }

        [TestMethod]
        public void Test_FindNonExistentBookByID()
        {
            Assert.ThrowsException<Exception>(() => libraryInstance.BorrowBook(testMember, 1000, DateTime.Now.AddDays(7)));
        }
    }
}
