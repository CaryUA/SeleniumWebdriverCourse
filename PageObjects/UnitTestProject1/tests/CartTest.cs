using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace LiteCart
{
    [TestFixture]
    public class CartTests : TestBase
    {
        [Test]
        public void ShoppingCartTesting()
        {
            app.AddToCart(3);
            app.RemoveFromCart(3);
        }
    }
}
