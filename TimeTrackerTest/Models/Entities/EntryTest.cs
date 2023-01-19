using TimeTracker.Models.Entities;

namespace TimeTrackerTest.Models.Entities;

public class EntryTest
{
    // ==============
    // Helping methods
    // ==============

    private Category GetTestCategory()
    {
        return new Category("My Category");
    }

    private Entry GetTestEntryWithSameDate(Category category, int pauseHours, int pauseMinutes)
    {
        return new Entry(
            category,
            new DateTime(2022, 12, 27, 15, 00, 00),
            new DateTime(2022, 12, 27, 18, 00, 00),
            new TimeSpan(pauseHours, pauseMinutes, 0),
            ""
        );
    }
    
    private Entry GetTestEntryWithDifferentDate(Category category, int pauseHours, int pauseMinutes)
    {
        return new Entry(
            category,
            new DateTime(2022, 12, 27, 19, 00, 00),
            new DateTime(2022, 12, 28, 00, 30, 00),
            new TimeSpan(pauseHours, pauseMinutes, 0),
            ""
        );
    }
    
    // ==============
    // Test methods
    // ==============

    [Test]
    [TestCase(0, 0)]
    [TestCase(0, 30)]
    [TestCase(1, 0)]
    [TestCase(1, 30)]
    public void GetTotalTime_SameDate(int pauseHours, int pauseMinutes)
    {
        Category category = this.GetTestCategory();
        Entry entry = this.GetTestEntryWithSameDate(category, pauseHours, pauseMinutes);

        TimeSpan pause = new TimeSpan(pauseHours, pauseMinutes, 0);
        TimeSpan targetTotal = new TimeSpan(3, 0, 0).Subtract(pause);
        TimeSpan realTotal = entry.TotalTime;
        
        Assert.That(realTotal, Is.EqualTo(targetTotal));
    }
    
    [Test]
    [TestCase(0, 0)]
    [TestCase(0, 30)]
    [TestCase(1, 0)]
    [TestCase(1, 30)]
    public void GetTotalTime_DifferentDate(int pauseHours, int pauseMinutes)
    {
        Category category = this.GetTestCategory();
        Entry entry = this.GetTestEntryWithDifferentDate(category, pauseHours, pauseMinutes);

        TimeSpan pause = new TimeSpan(pauseHours, pauseMinutes, 0);
        TimeSpan targetTotal = new TimeSpan(5, 30, 0).Subtract(pause);
        TimeSpan realTotal = entry.TotalTime;
        
        Assert.That(realTotal, Is.EqualTo(targetTotal));
    }
}