using System.Xml.Serialization;

namespace Pull_Projects
{
    [XmlRoot("Textbolds")]
    public class Textbolds
    {
        [XmlElement("Textbold")]
        public List<Textbold> TextboldList { get; set; }
    }

    public class Textbold
    {
        public string Width { get; set; }
    }
}

/*

 ||Serwis||alert_status||coverage||duplicated_lines_density||ncloc||bugs||vulnerabilities||code_smells||
|UserService|!https://sonar.example.com/api/project_badges/measure?project=project_userservice&metric=alert_status&token=xxx|height=20!|...|
...

 */

/*

 // 1. Mock for the scoped service (IDocumentsService)
    var documentsServiceMock = new Mock<IDocumentsService>();
    documentsServiceMock
        .Setup(x => x.GetDocumentLink())
        .ReturnsAsync("polska");

    // 2. Mock for IServiceProvider
    var serviceProviderMock = new Mock<IServiceProvider>();
    serviceProviderMock
        .Setup(sp => sp.GetService(typeof(IDocumentsService)))
        .Returns(documentsServiceMock.Object);

    // 3. Mock for IServiceScope
    var serviceScopeMock = new Mock<IServiceScope>();
    serviceScopeMock
        .Setup(s => s.ServiceProvider)
        .Returns(serviceProviderMock.Object);

    // 4. Mock for IServiceScopeFactory
    var scopeFactoryMock = new Mock<IServiceScopeFactory>();
    scopeFactoryMock
        .Setup(f => f.CreateScope())
        .Returns(serviceScopeMock.Object);

    // 5. Teraz wstrzykujesz scopeFactoryMock do klasy testowanej
    var sut = new DocumentLinkProvider(scopeFactoryMock.Object);

 */