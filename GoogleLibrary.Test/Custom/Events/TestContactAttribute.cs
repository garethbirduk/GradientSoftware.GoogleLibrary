using GoogleLibrary.Custom.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GoogleLibrary.Test.Custom.Events
{
    [TestClass]
    public class ContactAttributeTests
    {
        [TestMethod]
        public void AttributeUsage_InheritedIsFalse()
        {
            // Arrange & Act
            var attributeUsage = (AttributeUsageAttribute)Attribute.GetCustomAttribute(typeof(ContactAttribute), typeof(AttributeUsageAttribute));

            // Assert
            Assert.IsNotNull(attributeUsage);
            Assert.IsFalse(attributeUsage.Inherited);
        }

        [TestMethod]
        public void AttributeUsage_ValidOnAllTargets()
        {
            // Arrange & Act
            var attributeUsage = (AttributeUsageAttribute)Attribute.GetCustomAttribute(typeof(ContactAttribute), typeof(AttributeUsageAttribute));

            // Assert
            Assert.IsNotNull(attributeUsage);
            Assert.AreEqual(AttributeTargets.All, attributeUsage.ValidOn);
        }

        [TestMethod]
        public void Constructor_SetsEmailProperty()
        {
            // Arrange
            var email = "test@example.com";

            // Act
            var attribute = new ContactAttribute(email);

            // Assert
            Assert.AreEqual(email, attribute.Email);
        }

        [TestMethod]
        public void EmailProperty_ReturnsCorrectValue()
        {
            // Arrange
            var email = "contact@example.com";
            var attribute = new ContactAttribute(email);

            // Act
            var result = attribute.Email;

            // Assert
            Assert.AreEqual(email, result);
        }
    }
}