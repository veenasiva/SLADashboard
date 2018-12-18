using Microsoft.VisualStudio.TestTools.UnitTesting;
using SLADashboard.Controllers;
using SLADashboard.Models;
using SLADashboard.Core;
using SLADashboard.Core.Interfaces;
using SLADashboard.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Moq;

namespace SLADashboard.Controllers.Tests
{
     [TestClass()]
    public class OperatorControllerTests
    {
        private Mock<IClientsRepository> _clientRepositoryMock;
        private Mock<IProfilesRepository> _profileRepositoryMock;
        private Mock<ISystemConfigRepository> _systemConfigRepositoryMock;
        private Mock<ISLARepository> _slaRepositoryMock;
        private Mock<ISLAValuesRepository> _slaValuesRepositoryMock;
        private OperatorController controller;
        List<SLAValuesForSLA> lstSLAValues;

        [TestInitialize]
        public void Initialize()
        {
            _clientRepositoryMock = new Mock<IClientsRepository>();
            _profileRepositoryMock = new Mock<IProfilesRepository>();
            _systemConfigRepositoryMock = new Mock<ISystemConfigRepository>();
            _slaRepositoryMock = new Mock<ISLARepository>();
            _slaValuesRepositoryMock = new Mock<ISLAValuesRepository>();
            controller = new OperatorController(_clientRepositoryMock.Object, _profileRepositoryMock.Object, _systemConfigRepositoryMock.Object, _slaRepositoryMock.Object, _slaValuesRepositoryMock.Object);
            lstSLAValues = new List<SLAValuesForSLA>()
            {
                new SLAValuesForSLA(){ ProfileID=1,SLAID=1},
                new SLAValuesForSLA(){ID=2, ProfileID=6,SLAID=11,ReportingDate=new DateTime(2018,9,1),QuantityProcessed=650,QuantityOutsideofSLA=2}
            };

        }
        [TestMethod()]
        public void SLAValuesTableTest()
        {
            //Arrange
            var date = new DateTime(2018, 9, 1);
            _slaValuesRepositoryMock.Setup(x => x.GetSLAValues(1, date)).Returns(lstSLAValues);

            //Act
            var result = ((controller.SLAValuesTable(1, 2018, 9) as PartialViewResult).Model) as SLAValuesTableModel;

            //Assert
            Assert.AreEqual(result.SlaValues.Count, 2);
            Assert.AreEqual(1, result.SlaValues[0].ProfileID);
            Assert.AreEqual(6, result.SlaValues[1].ProfileID);
            Assert.AreEqual(650, result.SlaValues[1].QuantityProcessed);


        }
        [TestMethod()]
        public void InsertUpdateSLAValuesTest()
        {
            List<SLAValuesForSLA> slaValues = new List<SLAValuesForSLA>()
            {
                new SLAValuesForSLA(){ID=1, ProfileID=1,SLAID=1,ReportingDate=new DateTime(2018,9,1),QuantityProcessed=200,QuantityOutsideofSLA=2},
                new SLAValuesForSLA(){ ProfileID=6,SLAID=11,ReportingDate=new DateTime(2018,9,1),QuantityProcessed=250,QuantityOutsideofSLA=2}

            };
            //Arrange
            SLAValuesModel slas = new SLAValuesModel() { ProfileID=1,SelectedYear=2018,SelectedMonth=9, SlaValues=slaValues };

            //Act
            var result = (JsonResult)controller.SLAValues(slas);
            var res= result.Data as JsonResultReturn;
            //Assert 
            Assert.AreEqual(true, res.Success);
        }
    }
}