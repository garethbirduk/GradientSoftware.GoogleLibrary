using GoogleLibrary.Custom.Attributes;
using GoogleLibrary.Custom.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GoogleLibrary.Test.Custom.Events
{
    [TestClass]
    public class TypeAttributeTests
    {
        [TestMethod]
        public void AttributeUsage_InheritedIsFalse()
        {
            // Arrange & Act
            var attributeUsage = (AttributeUsageAttribute)Attribute.GetCustomAttribute(typeof(TypeAttribute), typeof(AttributeUsageAttribute));

            // Assert
            Assert.IsNotNull(attributeUsage);
            Assert.IsFalse(attributeUsage.Inherited);
        }

        [TestMethod]
        public void AttributeUsage_ValidOnAllTargets()
        {
            // Arrange & Act
            var attributeUsage = (AttributeUsageAttribute)Attribute.GetCustomAttribute(typeof(TypeAttribute), typeof(AttributeUsageAttribute));

            // Assert
            Assert.IsNotNull(attributeUsage);
            Assert.AreEqual(AttributeTargets.All, attributeUsage.ValidOn);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullType_ThrowsArgumentNullException()
        {
            // Act
            var attribute = new TypeAttribute(null);
        }

        [TestMethod]
        public void Constructor_SetsTypeProperty()
        {
            // Arrange
            var expectedType = typeof(string);

            // Act
            var attribute = new TypeAttribute(expectedType);

            // Assert
            Assert.AreEqual(expectedType, attribute.Type);
        }

        [TestMethod]
        public void EnumEventFieldType_PriceEuros_HasCorrectTypeAttribute()
        {
            // Arrange & Act
            var fieldInfo = typeof(EnumEventFieldType).GetField(nameof(EnumEventFieldType.PriceEuros));
            var typeAttribute = (TypeAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(TypeAttribute));

            // Assert
            Assert.IsNotNull(typeAttribute);
            Assert.AreEqual(typeof(Currency), typeAttribute.Type);
        }

        [TestMethod]
        public void EnumEventFieldType_PricePounds_HasCorrectTypeAttribute()
        {
            // Arrange & Act
            var fieldInfo = typeof(EnumEventFieldType).GetField(nameof(EnumEventFieldType.PricePounds));
            var typeAttribute = (TypeAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(TypeAttribute));

            // Assert
            Assert.IsNotNull(typeAttribute);
            Assert.AreEqual(typeof(Currency), typeAttribute.Type);
        }

        [TestMethod]
        public void EnumEventFieldType_StartDate_HasCorrectTypeAttribute()
        {
            // Arrange & Act
            var fieldInfo = typeof(EnumEventFieldType).GetField(nameof(EnumEventFieldType.StartDate));
            var typeAttribute = (TypeAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(TypeAttribute));

            // Assert
            Assert.IsNotNull(typeAttribute);
            Assert.AreEqual(typeof(DateTime), typeAttribute.Type);
        }

        [TestMethod]
        public void TypeProperty_ReturnsCorrectValue()
        {
            // Arrange
            var expectedType = typeof(int);
            var attribute = new TypeAttribute(expectedType);

            // Act
            var result = attribute.Type;

            // Assert
            Assert.AreEqual(expectedType, result);
        }
    }
}