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