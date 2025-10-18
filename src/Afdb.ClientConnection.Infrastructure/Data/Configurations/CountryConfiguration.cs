using Afdb.ClientConnection.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Afdb.ClientConnection.Infrastructure.Data.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<CountryEntity>
{
    public void Configure(EntityTypeBuilder<CountryEntity> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.NameFr).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Code).IsRequired().HasMaxLength(3);
        builder.Property(c => c.IsActive).HasDefaultValue(true);
        builder.HasIndex(c => c.Code).IsUnique();

        builder.HasData(
            new CountryEntity
            {
                Id = Guid.Parse("16bf844f-81e8-409b-b6c0-066d46ef2047"),
                Name = "Algeria",
                NameFr = "Algérie",
                Code = "DZ",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("55ffe5b5-0dce-4242-a1c9-6cc9b31c6b48"),
                Name = "Angola",
                NameFr = "Angola",
                Code = "AO",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("44a832f3-9a2c-4360-bd0c-d197d91c7ffc"),
                Name = "Benin",
                NameFr = "Bénin",
                Code = "BJ",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("f5b32999-f5f4-43a0-a005-03150707509e"),
                Name = "Botswana",
                NameFr = "Botswana",
                Code = "BW",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("45cb5526-d90f-4fb2-aecc-bea64e04cc61"),
                Name = "Burkina Faso",
                NameFr = "Burkina Faso",
                Code = "BF",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("278c7840-dd64-4733-b370-d06ba365e4bd"),
                Name = "Burundi",
                NameFr = "Burundi",
                Code = "BI",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("9f95189f-6be4-445e-8bed-11abd207398f"),
                Name = "Cabo Verde",
                NameFr = "Cap-Vert",
                Code = "CV",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("40d52c17-ddbb-4191-9aad-095dc1f3afec"),
                Name = "Cameroon",
                NameFr = "Cameroun",
                Code = "CM",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("999588cd-23b2-4624-8903-7c95adb94377"),
                Name = "Central African Republic",
                NameFr = "République centrafricaine",
                Code = "CF",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("eaa3d3ff-8a69-4207-8b9a-9fee511707a0"),
                Name = "Chad",
                NameFr = "Tchad",
                Code = "TD",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("06aac45f-afb8-4e16-bcb6-cb25f50f6c9a"),
                Name = "Comoros",
                NameFr = "Comores",
                Code = "KM",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("7f1f72df-fc52-468e-86dd-e10908252ab3"),
                Name = "Congo (Democratic Republic)",
                NameFr = "République démocratique du Congo",
                Code = "CD",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("f78bc4c9-5d9d-4f75-89ec-44f40034be09"),
                Name = "Congo",
                NameFr = "République du Congo",
                Code = "CG",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("4ec7011e-9148-4383-8906-9d7ba8ed4aea"),
                Name = "Côte d'Ivoire",
                NameFr = "Côte d'Ivoire",
                Code = "CI",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("2e593d13-82ed-4d18-80ea-26393f4071c2"),
                Name = "Djibouti",
                NameFr = "Djibouti",
                Code = "DJ",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("3f9dc2e8-6c08-4320-b408-70b197db214c"),
                Name = "Egypt",
                NameFr = "Égypte",
                Code = "EG",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("c9de1d01-bdf7-4ea2-8493-c7c3de1e8db6"),
                Name = "Equatorial Guinea",
                NameFr = "Guinée équatoriale",
                Code = "GQ",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("32bcc345-48b0-4b88-b1b5-bc4120e42943"),
                Name = "Eritrea",
                NameFr = "Érythrée",
                Code = "ER",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("4ba00dd3-e832-42d1-8db7-d1ddd82a4977"),
                Name = "Ethiopia",
                NameFr = "Éthiopie",
                Code = "ET",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("8256266a-c436-4e12-899f-c95781b053e1"),
                Name = "Gabon",
                NameFr = "Gabon",
                Code = "GA",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("8dddc3ca-8919-4c59-84cf-8a6af2f4144e"),
                Name = "Gambia",
                NameFr = "Gambie",
                Code = "GM",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("262a9bab-e842-4e51-b0c2-c4ab56184e2b"),
                Name = "Ghana",
                NameFr = "Ghana",
                Code = "GH",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("201e24e7-f4b4-4702-9556-2e2bcb5b2dc0"),
                Name = "Guinea",
                NameFr = "Guinée",
                Code = "GN",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("ff0551d8-5783-4b7a-a064-9bbb454e3c78"),
                Name = "Guinea-Bissau",
                NameFr = "Guinée-Bissau",
                Code = "GW",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("b8c61296-af91-4e53-89f3-f4d682b0350b"),
                Name = "Kenya",
                NameFr = "Kenya",
                Code = "KE",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("d62e490e-fc7a-43c8-aa32-05a1d0980110"),
                Name = "Lesotho",
                NameFr = "Lesotho",
                Code = "LS",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("3170066c-7064-4b49-af7e-dc87d8c9cbf2"),
                Name = "Liberia",
                NameFr = "Liberia",
                Code = "LR",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("bea01a0f-ae39-4d35-a15a-e84dacb23714"),
                Name = "Libya",
                NameFr = "Libye",
                Code = "LY",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("8ccc2bc5-8cb0-4e52-aa8f-f296d076f6a0"),
                Name = "Madagascar",
                NameFr = "Madagascar",
                Code = "MG",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("28f8e661-ef53-4266-baea-f943459ed3a0"),
                Name = "Malawi",
                NameFr = "Malawi",
                Code = "MW",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("93c5b5b9-a548-4ff2-8077-74f29bb41b78"),
                Name = "Mali",
                NameFr = "Mali",
                Code = "ML",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("3239c809-8568-4a03-960f-4b0f568a3ada"),
                Name = "Mauritania",
                NameFr = "Mauritanie",
                Code = "MR",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("5c82037c-98be-4d65-8fbb-4ae7d615caa2"),
                Name = "Mauritius",
                NameFr = "Maurice",
                Code = "MU",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("3e1e98d5-e342-45af-8919-8d27caeddd11"),
                Name = "Morocco",
                NameFr = "Maroc",
                Code = "MA",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("ac9109d0-1c94-46b4-b1c5-95cecf53304e"),
                Name = "Mozambique",
                NameFr = "Mozambique",
                Code = "MZ",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("c941d4ab-cd7d-4225-9113-1ec2009bd627"),
                Name = "Namibia",
                NameFr = "Namibie",
                Code = "NA",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("57d2a2c0-97f2-4d76-8509-335e85deea75"),
                Name = "Niger",
                NameFr = "Niger",
                Code = "NE",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("143fb95d-f41d-4af3-ac51-10f720f546a8"),
                Name = "Nigeria",
                NameFr = "Nigeria",
                Code = "NG",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("ddd70b66-ef0a-40be-9bf9-c601aa5ba34d"),
                Name = "Rwanda",
                NameFr = "Rwanda",
                Code = "RW",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("5d290047-6a5c-473e-aa21-9fe0a6db6376"),
                Name = "Sao Tome and Principe",
                NameFr = "Sao Tomé-et-Principe",
                Code = "ST",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("337112f4-e35e-44b8-9659-15dfafe0aa55"),
                Name = "Senegal",
                NameFr = "Sénégal",
                Code = "SN",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("2fdcdcb5-2f19-4232-a8d0-b80fa4e59dd1"),
                Name = "Seychelles",
                NameFr = "Seychelles",
                Code = "SC",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("20c99637-afcc-44d1-843e-c5f1866d0dd2"),
                Name = "Sierra Leone",
                NameFr = "Sierra Leone",
                Code = "SL",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("e64b4cd1-b88c-4d8f-9560-7a2afba4276e"),
                Name = "Somalia",
                NameFr = "Somalie",
                Code = "SO",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("45a74a51-ce4c-419e-a897-736c80c98ac3"),
                Name = "South Africa",
                NameFr = "Afrique du Sud",
                Code = "ZA",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("88a7942f-42da-488d-9cbf-1651744d8fe7"),
                Name = "South Sudan",
                NameFr = "Soudan du Sud",
                Code = "SS",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("54c29f69-8e0e-4b4c-a937-301a3507c1ac"),
                Name = "Sudan",
                NameFr = "Soudan",
                Code = "SD",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("b8951b42-af7f-40e5-bc53-704d1c5cdf57"),
                Name = "Tanzania",
                NameFr = "Tanzanie",
                Code = "TZ",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("456881e5-2731-447a-8221-41f59cadf64a"),
                Name = "Togo",
                NameFr = "Togo",
                Code = "TG",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("50de7424-4bb4-4f18-81d4-8c445f3c1571"),
                Name = "Tunisia",
                NameFr = "Tunisie",
                Code = "TN",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("6bc83388-ec40-48e0-bc3d-2c6b7111cafa"),
                Name = "Uganda",
                NameFr = "Ouganda",
                Code = "UG",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("1109d7e7-2f27-4da3-b3e8-46c2518412a7"),
                Name = "Zambia",
                NameFr = "Zambie",
                Code = "ZM",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("034d456d-1003-4b0f-8a10-826d6284a514"),
                Name = "Zimbabwe",
                NameFr = "Zimbabwe",
                Code = "ZW",
                IsActive = true
            },
            new CountryEntity
            {
                Id = Guid.Parse("91431d44-531f-48d1-b990-8bd34aa185f4"),
                Name = "Not Defined",
                NameFr = "Non défini",
                Code = "NOT",
                IsActive = false
            });
    }

}
