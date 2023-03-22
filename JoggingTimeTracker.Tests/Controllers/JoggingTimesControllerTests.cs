using AutoMapper;
using JoggingTimeTracker.API.Controllers;
using JoggingTimeTracker.API.Utils;
using JoggingTimeTracker.Core.Interfaces;
using JoggingTimeTracker.Core.Models;
using JoggingTimeTracker.Core.Models.Report;
using JoggingTimeTracker.Core.Models.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace JoggingTimeTracker.Tests.Controllers
{
    [TestClass]
    public class JoggingTimesControllerTests
    {
        private readonly Mock<IJoggingTimeRepository> _joggingTimeRepositoryMock;
        private readonly IMapper _mapper;
        private readonly JoggingTimesController _controller;

        public JoggingTimesControllerTests()
        {
            _joggingTimeRepositoryMock = new Mock<IJoggingTimeRepository>();
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()));
            _controller = new JoggingTimesController(_joggingTimeRepositoryMock.Object, _mapper);
        }

        [TestMethod]
        public async Task GetJoggingTimesFilterDate_ReturnsOk()
        {
            // Arrange
            string userId = "testUserId";
            DateTime fromDate = DateTime.Now.AddDays(-7);
            DateTime toDate = DateTime.Now;
            var mockResult = new ResultWithData<IEnumerable<JoggingTime>>.Success(new List<WeeklyJoggingStats>());
            
            _joggingTimeRepositoryMock.Setup(x => x.GetJoggingTimesFilterDate(userId, fromDate, toDate))
                .ReturnsAsync(mockResult);

            // Act
            var result = await _controller.GetJoggingTimesFilterDate(fromDate, toDate);

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(mockResult, (result.Result as OkObjectResult).Value);
        }

        [TestMethod]
        public async Task GetAllJoggingTimes_ReturnsBadRequest()
        {
            // Arrange
            string userId = "testUserId";
            var mockResult = new ResultWithData<IEnumerable<JoggingTime>>.Error("An error occurred");
            
            _joggingTimeRepositoryMock.Setup(x => x.GetAllJoggingTimes(userId))
                .ReturnsAsync(mockResult);

            // Act
            var result = await _controller.GetAllJoggingTimes();

            // Assert
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(mockResult, (result.Result as BadRequestObjectResult).Value);
        }

        // Add more unit tests for the other controller actions
    }


}
