using Afdb.ClientConnection.Infrastructure.Data.Entities;

namespace Afdb.ClientConnection.Infrastructure.Data.Seeder;

internal static class DisbursementPermissionSeeder
{
    public static IEnumerable<DisbursementPermissionEntity> GetData()
    {
        const string systemUser = "System";

        return new List<DisbursementPermissionEntity>
        {
            //For Excuting agencies
            new DisbursementPermissionEntity
            {
                Id = new Guid("aa7e6455-4596-40fe-8f70-495d2f4dba08"),
                BusinessProfileId = new Guid("d8a54872-2e74-4de1-8f07-76d8e8275e01"),
                FunctionId = new Guid("1a2b3c4d-0001-4e5f-9012-abcdef123456"),
                CanConsult = true,
                CanSubmit = false,
                CreatedAt = new DateTime(2025,11,1,0,0,0,DateTimeKind.Utc),
                CreatedBy = systemUser
            },
            new DisbursementPermissionEntity
            {
                Id = new Guid("26f59882-bcf8-4bcf-8e50-823d19f15b4a"),
                BusinessProfileId = new Guid("d8a54872-2e74-4de1-8f07-76d8e8275e01"),
                FunctionId = new Guid("1a2b3c4d-0002-4e5f-9012-abcdef123456"),
                CanConsult = true,
                CanSubmit = true,
                CreatedAt = new DateTime(2025,11,1,0,0,0,DateTimeKind.Utc),
                CreatedBy = systemUser
            },
            new DisbursementPermissionEntity
            {
                Id = new Guid("4f041a12-bcfb-43fc-a237-a6c4b4ba56bf"),
                BusinessProfileId = new Guid("d8a54872-2e74-4de1-8f07-76d8e8275e01"),
                FunctionId = new Guid("1a2b3c4d-0003-4e5f-9012-abcdef123456"),
                CanConsult = false,
                CanSubmit = false,
                CreatedAt = new DateTime(2025,11,1,0,0,0,DateTimeKind.Utc),
                CreatedBy = systemUser
            },
            new DisbursementPermissionEntity
            {
                Id = new Guid("a25e56c8-3d75-4f79-a77e-16f68a8aef01"),
                BusinessProfileId = new Guid("d8a54872-2e74-4de1-8f07-76d8e8275e01"),
                FunctionId = new Guid("1a2b3c4d-0004-4e5f-9012-abcdef123456"),
                CanConsult = true,
                CanSubmit = true,
                CreatedAt = new DateTime(2025,11,1,0,0,0,DateTimeKind.Utc),
                CreatedBy = systemUser
            },
            new DisbursementPermissionEntity
            {
                Id = new Guid("0cf86a66-7a42-4e50-b2b7-a61054d12c3f"),
                BusinessProfileId = new Guid("d8a54872-2e74-4de1-8f07-76d8e8275e01"),
                FunctionId = new Guid("1a2b3c4d-0005-4e5f-9012-abcdef123456"),
                CanConsult = true,
                CanSubmit = true,
                CreatedAt = new DateTime(2025,11,1,0,0,0,DateTimeKind.Utc),
                CreatedBy = systemUser
            },
            new DisbursementPermissionEntity
            {
                Id = new Guid("5fba02aa-b580-4063-97f1-35c9e809c376"),
                BusinessProfileId = new Guid("d8a54872-2e74-4de1-8f07-76d8e8275e01"),
                FunctionId = new Guid("1a2b3c4d-0006-4e5f-9012-abcdef123456"),
                CanConsult = true,
                CanSubmit = false,
                CreatedAt = new DateTime(2025,11,1,0,0,0,DateTimeKind.Utc),
                CreatedBy = systemUser
            },
        
            //For Borrower
            new DisbursementPermissionEntity
            {
                Id = new Guid("4ff93882-e9fe-4ce8-a05a-d2b580fca08d"),
                BusinessProfileId = new Guid("2d42d812-7d9d-4c44-9c55-b93db6a9a21b"),
                FunctionId = new Guid("1a2b3c4d-0001-4e5f-9012-abcdef123456"),
                CanConsult = true,
                CanSubmit = false,
                CreatedAt = new DateTime(2025,11,1,0,0,0,DateTimeKind.Utc),
                CreatedBy = systemUser
            },
            new DisbursementPermissionEntity
            {
                Id = new Guid("09756fce-635d-4f08-8b85-1736422cba0c"),
                BusinessProfileId = new Guid("2d42d812-7d9d-4c44-9c55-b93db6a9a21b"),
                FunctionId = new Guid("1a2b3c4d-0002-4e5f-9012-abcdef123456"),
                CanConsult = true,
                CanSubmit = false,
                CreatedAt = new DateTime(2025,11,1,0,0,0,DateTimeKind.Utc),
                CreatedBy = systemUser
            },
            new DisbursementPermissionEntity
            {
                Id = new Guid("8a8a0293-423c-44ec-9232-4425f89fb462"),
                BusinessProfileId = new Guid("2d42d812-7d9d-4c44-9c55-b93db6a9a21b"),
                FunctionId = new Guid("1a2b3c4d-0003-4e5f-9012-abcdef123456"),
                CanConsult = true,
                CanSubmit = true,
                CreatedAt = new DateTime(2025,11,1,0,0,0,DateTimeKind.Utc),
                CreatedBy = systemUser
            },
            new DisbursementPermissionEntity
            {
                Id = new Guid("9854db95-9043-44e6-b434-0bdde60127f0"),
                BusinessProfileId = new Guid("2d42d812-7d9d-4c44-9c55-b93db6a9a21b"),
                FunctionId = new Guid("1a2b3c4d-0004-4e5f-9012-abcdef123456"),
                CanConsult = false,
                CanSubmit = false,
                CreatedAt = new DateTime(2025,11,1,0,0,0,DateTimeKind.Utc),
                CreatedBy = systemUser
            },
            new DisbursementPermissionEntity
            {
                Id = new Guid("1e594e22-e012-45fd-88e3-1daa265f13fa"),
                BusinessProfileId = new Guid("2d42d812-7d9d-4c44-9c55-b93db6a9a21b"),
                FunctionId = new Guid("1a2b3c4d-0005-4e5f-9012-abcdef123456"),
                CanConsult = false,
                CanSubmit = false,
                CreatedAt = new DateTime(2025,11,1,0,0,0,DateTimeKind.Utc),
                CreatedBy = systemUser
            },
            new DisbursementPermissionEntity
            {
                Id = new Guid("c58e8741-5bfe-4073-a136-1688cf0f7ce5"),
                BusinessProfileId = new Guid("2d42d812-7d9d-4c44-9c55-b93db6a9a21b"),
                FunctionId = new Guid("1a2b3c4d-0006-4e5f-9012-abcdef123456"),
                CanConsult = true,
                CanSubmit = false,
                CreatedAt = new DateTime(2025,11,1,0,0,0,DateTimeKind.Utc),
                CreatedBy = systemUser
            },
       
            //For Guarantor
            new DisbursementPermissionEntity
            {
                Id = new Guid("b07960b3-57cf-45cd-9bd9-90072cfa1c49"),
                BusinessProfileId = new Guid("a18f4194-b0fb-4682-8a4e-3f3991a9b632"),
                FunctionId = new Guid("1a2b3c4d-0001-4e5f-9012-abcdef123456"),
                CanConsult = true,
                CanSubmit = false,
                CreatedAt = new DateTime(2025,11,1,0,0,0,DateTimeKind.Utc),
                CreatedBy = systemUser
            },
            new DisbursementPermissionEntity
            {
                Id = new Guid("49521407-96c9-4cc7-b999-b41e19ab9607"),
                BusinessProfileId = new Guid("a18f4194-b0fb-4682-8a4e-3f3991a9b632"),
                FunctionId = new Guid("1a2b3c4d-0002-4e5f-9012-abcdef123456"),
                CanConsult = false,
                CanSubmit = false,
                CreatedAt = new DateTime(2025,11,1,0,0,0,DateTimeKind.Utc),
                CreatedBy = systemUser
            },
            new DisbursementPermissionEntity
            {
                Id = new Guid("f771857e-fa6b-4f0a-a13c-2d3c032c9d4f"),
                BusinessProfileId = new Guid("a18f4194-b0fb-4682-8a4e-3f3991a9b632"),
                FunctionId = new Guid("1a2b3c4d-0003-4e5f-9012-abcdef123456"),
                CanConsult = true,
                CanSubmit = false,
                CreatedAt = new DateTime(2025,11,1,0,0,0,DateTimeKind.Utc),
                CreatedBy = systemUser
            },
            new DisbursementPermissionEntity
            {
                Id = new Guid("4b48131f-905d-4506-9507-b5d04cdb8838"),
                BusinessProfileId = new Guid("a18f4194-b0fb-4682-8a4e-3f3991a9b632"),
                FunctionId = new Guid("1a2b3c4d-0004-4e5f-9012-abcdef123456"),
                CanConsult = false,
                CanSubmit = false,
                CreatedAt = new DateTime(2025,11,1,0,0,0,DateTimeKind.Utc),
                CreatedBy = systemUser
            },
            new DisbursementPermissionEntity
            {
                Id = new Guid("ad736711-7595-499c-a8ab-2c2c2cd698d2"),
                BusinessProfileId = new Guid("a18f4194-b0fb-4682-8a4e-3f3991a9b632"),
                FunctionId = new Guid("1a2b3c4d-0005-4e5f-9012-abcdef123456"),
                CanConsult = false,
                CanSubmit = false,
                CreatedAt = new DateTime(2025,11,1,0,0,0,DateTimeKind.Utc),
                CreatedBy = systemUser
            },
            new DisbursementPermissionEntity
            {
                Id = new Guid("a447a675-f46e-4861-878b-1bd897afe690"),
                BusinessProfileId = new Guid("a18f4194-b0fb-4682-8a4e-3f3991a9b632"),
                FunctionId = new Guid("1a2b3c4d-0006-4e5f-9012-abcdef123456"),
                CanConsult = true,
                CanSubmit = false,
                CreatedAt = new DateTime(2025,11,1,0,0,0,DateTimeKind.Utc),
                CreatedBy = systemUser
            },
        };
    }
}
