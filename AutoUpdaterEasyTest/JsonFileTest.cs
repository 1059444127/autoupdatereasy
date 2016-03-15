using System;
using AutoUpdaterEasy.Entities;
using AutoUpdaterEasy.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoUpdaterEasyTest
{
    [TestClass]
    public class JsonFileTest
    {
        [TestMethod]
        public void ShouldFactoryAClass()
        {
            var file = JsonConfig.Factory("http://update.arcnet.com.br/autoupdatereasytest/myjsonupdater.json");
            Assert.IsNotNull(file);
        }

        [TestMethod]
        [ExpectedException(typeof(ProtocolErrorException))]
        public void JsonNotFound()
        {
            JsonConfig.Factory("http://update.arcnet.com.br/autoupdatereasytest/myjsonupdater1.json");            
        }

        [TestMethod]
        [ExpectedException(typeof(DnsNotResolveException))]
        public void DnsNotResolve()
        {
            JsonConfig.Factory("http://update1.arcnet.com.br/autoupdatereasytest/myjsonupdater1.json");
        }
    }
}
