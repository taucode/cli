﻿using NUnit.Framework;
using TauCode.Cli.Exceptions;
using TauCode.Cli.Tests.Common.BadHosts;
using TauCode.Parsing;

namespace TauCode.Cli.Tests.UnitTests.BadHosts
{
    [TestFixture]
    public class BadHostsTests
    {
        [Test]
        public void Constructor_HostWithoutAddIns_RunsOk()
        {
            // Arrange
            
            // Act
            var host = new HostWithoutAddIns();
            var dummy = host.Node;

            // Assert
            var addIns = host.GetAddIns();
            Assert.That(addIns, Is.Empty);
            Assert.Pass();
        }

        [Test]
        public void Constructor_HostWithEmptyAddIn_ThrowsCliException()
        {
            // Arrange

            // Act
            var host = new HostWithEmptyAddIn();

            INode dummy;
            var ex = Assert.Throws<CliException>(() => dummy = host.Node);

            // Assert
            Assert.That(ex.Message, Is.EqualTo("'CreateWorkers' must not return empty collection."));
        }
    }
}
