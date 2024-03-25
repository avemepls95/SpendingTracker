using FluentAssertions;
using SpendingTracker.GenericSubDomain.Common;
using SpendingTracker.TelegramBot.TextMessageParsing.Internal;
using Xunit;

namespace SpendingTracker.TelegramBot.Internal.Tests;

public class SpendingMessagePartParserTests
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("123245")]
    [InlineData("asdasdas")]
    [InlineData("50.04.2021")]
    [InlineData("1.13.2021")]
    [InlineData("1.1.12345")]
    public void ParseDateAsUts_InvalidInput_ShouldThrowException(string input)
    {
        // act
        var result = TextMessagePartParser.ParseDate(input);

        // assert
        result.IsSuccess.Should().Be(false);
        result.ErrorMessage.Should().Contain("Не удалось распознать дату. Доступные форматы ввода");
    }

    public static readonly object[][] ParseDateTestData =
    {
        new object[] { "2/2/2022", new DateTimeOffset(new DateTime(2022, 2, 2)) },
        new object[] { "22/2/2022", new DateTimeOffset(new DateTime(2022, 2, 22)) },
        new object[] { "2/12/2022", new DateTimeOffset(new DateTime(2022, 12, 2)) },
        new object[] { "22/12/2022", new DateTimeOffset(new DateTime(2022, 12, 22)) },
        new object[] { "2/2/22", new DateTimeOffset(new DateTime(2022, 2, 2)) },
        new object[] { "22/2/22", new DateTimeOffset(new DateTime(2022, 2, 22)) },
        new object[] { "2/12/22", new DateTimeOffset(new DateTime(2022, 12, 2)) },
        new object[] { "22/12/22", new DateTimeOffset(new DateTime(2022, 12, 22)) },
        
        new object[] { "2.2.2022", new DateTimeOffset(new DateTime(2022, 2, 2)) },
        new object[] { "22.2.2022", new DateTimeOffset(new DateTime(2022, 2, 22)) },
        new object[] { "2.12.2022", new DateTimeOffset(new DateTime(2022, 12, 2)) },
        new object[] { "22.12.2022", new DateTimeOffset(new DateTime(2022, 12, 22)) },
        new object[] { "2.2.22", new DateTimeOffset(new DateTime(2022, 2, 2)) },
        new object[] { "22.2.22", new DateTimeOffset(new DateTime(2022, 2, 22)) },
        new object[] { "2.12.22", new DateTimeOffset(new DateTime(2022, 12, 2)) },
        new object[] { "22.12.22", new DateTimeOffset(new DateTime(2022, 12, 22)) },
        
        new object[] { "2/2", new DateTimeOffset(new DateTime(DateTime.UtcNow.Year, 2, 2)) },
        new object[] { "22/2", new DateTimeOffset(new DateTime(DateTime.UtcNow.Year, 2, 22)) }, 
        new object[] { "2/12", new DateTimeOffset(new DateTime(DateTime.UtcNow.Year, 12, 2)) },
        new object[] { "22/12", new DateTimeOffset(new DateTime(DateTime.UtcNow.Year, 12, 22)) },
        new object[] { "2/2", new DateTimeOffset(new DateTime(DateTime.UtcNow.Year, 2, 2)) },
        new object[] { "22/2", new DateTimeOffset(new DateTime(DateTime.UtcNow.Year, 2, 22)) },
        new object[] { "2/12", new DateTimeOffset(new DateTime(DateTime.UtcNow.Year, 12, 2)) },
        new object[] { "22/12", new DateTimeOffset(new DateTime(DateTime.UtcNow.Year, 12, 22)) },
        
        new object[] { "2.2",new DateTimeOffset( new DateTime(DateTime.UtcNow.Year, 2, 2)) },
        new object[] { "22.2", new DateTimeOffset(new DateTime(DateTime.UtcNow.Year, 2, 22)) },
        new object[] { "2.12", new DateTimeOffset(new DateTime(DateTime.UtcNow.Year, 12, 2)) },
        new object[] { "22.12", new DateTimeOffset(new DateTime(DateTime.UtcNow.Year, 12, 22)) },
        new object[] { "2.2", new DateTimeOffset(new DateTime(DateTime.UtcNow.Year, 2, 2)) },
        new object[] { "22.2", new DateTimeOffset(new DateTime(DateTime.UtcNow.Year, 2, 22)) },
        new object[] { "2.12", new DateTimeOffset(new DateTime(DateTime.UtcNow.Year, 12, 2)) },
        new object[] { "30.09", new DateTimeOffset(new DateTime(DateTime.UtcNow.Year, 9, 30)) }
    };

    [Theory, MemberData(nameof(ParseDateTestData))]
    public void ParseDateAsUts_ValidInput_ShouldParse(string input, DateTimeOffset expectedResultAsLocal)
    {
        // act
        var result = TextMessagePartParser.ParseDate(input);

        // assert
        result.IsSuccess.Should().Be(true);
        result.ErrorMessage.Should().BeNullOrEmpty();

        // var expectedResultAsUtc = new DateTime(expectedResultAsLocal.ToUniversalTime().Date, TimeSpan.Zero);
        result.Result.Should().Be(expectedResultAsLocal);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("1.2.3")]
    [InlineData("1111111111111111111111111111111111111111111111111111111111122222222222222222222222222222222")]
    [InlineData("asdasdas")]
    public void ParseAmount_InputNotNumber_ShouldThrowException(string input)
    {
        // act
        var result = TextMessagePartParser.ParseAmount(input);

        // assert
        result.IsSuccess.Should().Be(false);
        result.ErrorMessage.Should()
            .Be("Не удалось распознать сумму. Возможно введено не число, либо число слишком большое");
    }

    [Theory]
    [InlineData("-1")]
    [InlineData("0")]
    public void ParseAmount_AmountLessThanOne_ShouldThrowException(string input)
    {
        // act
        var result = TextMessagePartParser.ParseAmount(input);

        // assert
        result.IsSuccess.Should().Be(false);
        result.ErrorMessage.Should().Be("Значение суммы должно быть больше 0");
    }

    [Theory]
    [InlineData("1", 1)]
    [InlineData("2.234", 2.234)]
    [InlineData("2,234", 2.234)]
    public void ParseAmount_CorrectAmount_ShouldParse(string input, float expectedResult)
    {
        // act
        var result = TextMessagePartParser.ParseAmount(input);

        // assert
        result.IsSuccess.Should().Be(true);
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.Result.IsEqual(expectedResult).Should().BeTrue();
    }
}