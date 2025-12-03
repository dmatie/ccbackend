# Guide de Sécurité - Validation des Entrées Utilisateur

## 📋 Vue d'ensemble

Ce guide explique comment utiliser le système de validation de sécurité pour protéger l'application contre:
- **XSS (Cross-Site Scripting)**
- **Injection SQL**
- **Caractères dangereux**
- **Contenu malveillant**

## 🔧 Architecture

### Composants

1. **`IInputSanitizationService`** - Interface pour la sanitization
2. **`InputSanitizationService`** - Implémentation utilisant HtmlSanitizer
3. **`SecurityValidationExtensions`** - Extensions FluentValidation réutilisables

## 🛡️ Utilisation dans les Validators

### 1. Injection du Service

```csharp
public class CreateClaimCommandValidator : AbstractValidator<CreateClaimCommand>
{
    private readonly IInputSanitizationService _sanitizationService;

    public CreateClaimCommandValidator(IInputSanitizationService sanitizationService)
    {
        _sanitizationService = sanitizationService;

        ConfigureValidationRules();
    }

    private void ConfigureValidationRules()
    {
        // Vos règles de validation ici
    }
}
```

### 2. Validation des Noms et Identifiants

Pour les champs comme FirstName, LastName, CompanyName:

```csharp
RuleFor(x => x.FirstName)
    .NotEmpty()
    .WithMessage("ERR.Validation.FirstNameRequired")
    .MaximumLength(100)
    .WithMessage("ERR.Validation.FirstNameTooLong")
    .SafeName(_sanitizationService);  // ✅ Protection complète
```

**Ce que fait `.SafeName()`:**
- Détecte les patterns XSS
- Détecte les patterns d'injection SQL
- Supprime les espaces en début/fin
- Bloque les balises script

### 3. Validation des Descriptions et Textes Longs

Pour les champs comme Description, Comments, Notes:

```csharp
RuleFor(x => x.Description)
    .MaximumLength(2000)
    .WithMessage("ERR.Validation.DescriptionTooLong")
    .SafeDescription(_sanitizationService);  // ✅ Protection description
```

**Ce que fait `.SafeDescription()`:**
- Bloque le contenu dangereux (XSS, injection)
- Bloque les balises script
- Vérifie les caractères de contrôle
- Détecte les espaces excessifs

### 4. Validation des Noms de Fichiers

```csharp
RuleFor(x => x.FileName)
    .NotEmpty()
    .WithMessage("ERR.Validation.FileNameRequired")
    .SafeFileName(_sanitizationService);  // ✅ Protection fichiers
```

**Ce que fait `.SafeFileName()`:**
- Bloque les caractères invalides (/, \, :, etc.)
- Bloque la traversée de répertoires (..)
- Bloque les noms réservés Windows (CON, PRN, etc.)
- Détecte le contenu dangereux

### 5. Validation des URLs

```csharp
RuleFor(x => x.WebsiteUrl)
    .SafeUrl(_sanitizationService);  // ✅ Protection URLs
```

**Ce que fait `.SafeUrl()`:**
- Vérifie le format d'URL valide
- Autorise uniquement HTTP/HTTPS
- Détecte les patterns XSS dans l'URL

### 6. Validation Alphanumérique

Pour les codes, identifiants:

```csharp
RuleFor(x => x.ProjectCode)
    .AlphanumericWithSpaces(allowDashes: true, allowUnderscores: true);
```

Options:
- `allowDashes: true` → Autorise les tirets (-)
- `allowUnderscores: true` → Autorise les underscores (_)

### 7. Validations Spécifiques

#### Bloquer uniquement XSS
```csharp
RuleFor(x => x.Content)
    .NoXssContent(_sanitizationService);
```

#### Bloquer uniquement SQL Injection
```csharp
RuleFor(x => x.SearchTerm)
    .NoSqlInjection(_sanitizationService);
```

#### Bloquer tout contenu dangereux
```csharp
RuleFor(x => x.UserInput)
    .NoDangerousContent(_sanitizationService);
```

#### Bloquer les balises script
```csharp
RuleFor(x => x.HtmlContent)
    .NoScriptTags();
```

#### Vérifier les espaces
```csharp
RuleFor(x => x.Name)
    .NoLeadingTrailingWhitespace();
```

## 📝 Exemples de Validators Complets

### Exemple 1: Création d'un Utilisateur

```csharp
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly IInputSanitizationService _sanitizationService;

    public CreateUserCommandValidator(IInputSanitizationService sanitizationService)
    {
        _sanitizationService = sanitizationService;

        // Email (pas besoin de validation supplémentaire, EmailAddress() suffit)
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("ERR.User.EmailRequired")
            .EmailAddress()
            .WithMessage("ERR.User.InvalidEmail")
            .MaximumLength(255)
            .WithMessage("ERR.User.EmailTooLong");

        // Prénom - protection complète
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("ERR.User.FirstNameRequired")
            .MaximumLength(100)
            .WithMessage("ERR.User.FirstNameTooLong")
            .SafeName(_sanitizationService);

        // Nom - protection complète
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("ERR.User.LastNameRequired")
            .MaximumLength(100)
            .WithMessage("ERR.User.LastNameTooLong")
            .SafeName(_sanitizationService);
    }
}
```

### Exemple 2: Upload de Fichier

```csharp
public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
{
    private readonly IInputSanitizationService _sanitizationService;

    public UploadFileCommandValidator(IInputSanitizationService sanitizationService)
    {
        _sanitizationService = sanitizationService;

        RuleFor(x => x.FileName)
            .NotEmpty()
            .WithMessage("ERR.File.NameRequired")
            .MaximumLength(255)
            .WithMessage("ERR.File.NameTooLong")
            .SafeFileName(_sanitizationService);

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .WithMessage("ERR.File.DescriptionTooLong")
            .SafeDescription(_sanitizationService)
            .When(x => !string.IsNullOrWhiteSpace(x.Description));
    }
}
```

### Exemple 3: Création de Claim

```csharp
public class CreateClaimCommandValidator : AbstractValidator<CreateClaimCommand>
{
    private readonly IInputSanitizationService _sanitizationService;

    public CreateClaimCommandValidator(IInputSanitizationService sanitizationService)
    {
        _sanitizationService = sanitizationService;

        RuleFor(x => x.Subject)
            .NotEmpty()
            .WithMessage("ERR.Claim.SubjectRequired")
            .MaximumLength(200)
            .WithMessage("ERR.Claim.SubjectTooLong")
            .SafeName(_sanitizationService);

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("ERR.Claim.DescriptionRequired")
            .MaximumLength(2000)
            .WithMessage("ERR.Claim.DescriptionTooLong")
            .SafeDescription(_sanitizationService);
    }
}
```

## 🔍 Utilisation Directe du Service

Si vous devez nettoyer des entrées manuellement:

```csharp
public class MyService
{
    private readonly IInputSanitizationService _sanitizationService;

    public MyService(IInputSanitizationService sanitizationService)
    {
        _sanitizationService = sanitizationService;
    }

    public string ProcessUserInput(string input)
    {
        // Nettoyer le HTML (garde uniquement les balises sûres)
        var cleanHtml = _sanitizationService.SanitizeHtml(input);

        // Nettoyer complètement (supprime tout HTML)
        var cleanText = _sanitizationService.SanitizeText(input);

        // Vérifier si contient du contenu dangereux
        if (_sanitizationService.ContainsDangerousContent(input))
        {
            throw new ValidationException("Contenu dangereux détecté");
        }

        return cleanText;
    }
}
```

## 📊 Matrice de Validation par Type de Champ

| Type de Champ | Validation Recommandée | Exemple |
|--------------|----------------------|---------|
| Nom/Prénom | `.SafeName()` | FirstName, LastName |
| Email | `.EmailAddress()` | Email |
| Description | `.SafeDescription()` | Description, Comments |
| Nom de fichier | `.SafeFileName()` | FileName |
| URL | `.SafeUrl()` | WebsiteUrl |
| Code/ID | `.AlphanumericWithSpaces()` | ProjectCode |
| Texte libre | `.NoDangerousContent()` | FreeText |
| Recherche | `.NoSqlInjection()` | SearchTerm |

## ⚠️ Champs à NE PAS Sur-Valider

### 1. Emails
❌ **Ne pas faire:**
```csharp
RuleFor(x => x.Email)
    .EmailAddress()
    .SafeName(_sanitizationService);  // ❌ Inutile, EmailAddress() suffit
```

✅ **À faire:**
```csharp
RuleFor(x => x.Email)
    .EmailAddress()  // ✅ Suffisant
    .MaximumLength(255);
```

### 2. GUIDs
❌ **Ne pas faire:**
```csharp
RuleFor(x => x.UserId)
    .SafeName(_sanitizationService);  // ❌ Un GUID est déjà sûr
```

✅ **À faire:**
```csharp
RuleFor(x => x.UserId)
    .NotEmpty();  // ✅ Suffisant
```

### 3. Nombres
❌ **Ne pas faire:**
```csharp
RuleFor(x => x.Age)
    .SafeName(_sanitizationService);  // ❌ Un int est déjà typé
```

✅ **À faire:**
```csharp
RuleFor(x => x.Age)
    .GreaterThan(0)  // ✅ Validation métier
    .LessThan(150);
```

## 🎯 Bonnes Pratiques

### 1. Validation en Couches
- ✅ Validation au niveau du Command Validator (FluentValidation)
- ✅ Validation au niveau de l'entité (Domain)
- ✅ Ne pas dupliquer inutilement

### 2. Messages d'Erreur Clairs
```csharp
.SafeName(_sanitizationService)
.WithMessage("ERR.Validation.InvalidName");  // ✅ Message clair
```

### 3. Autoriser les Caractères Légitimes
```csharp
// ✅ Autorise les accents, apostrophes dans les noms
.SafeName(_sanitizationService)

// ❌ Ne pas restreindre trop
.Matches("^[a-z]+$")  // Bloque les accents !
```

### 4. Validation Conditionnelle
```csharp
RuleFor(x => x.OptionalField)
    .SafeName(_sanitizationService)
    .When(x => !string.IsNullOrWhiteSpace(x.OptionalField));  // ✅
```

## 🚨 Patterns Détectés

### XSS Patterns
- `<script>...</script>`
- `javascript:...`
- `onerror=`, `onload=`, etc.
- `eval(...)`
- `<iframe>`, `<embed>`, `<object>`

### SQL Injection Patterns
- `OR 1=1`
- `'; DROP TABLE`
- `UNION SELECT`
- `--` (commentaires SQL)
- `xp_cmdshell`, `sp_executesql`

### Caractères Dangereux
- Caractères de contrôle (sauf \n, \r, \t)
- Traversée de répertoires (`..`)
- Noms de fichiers réservés Windows

## 📦 Migration des Validators Existants

Pour migrer un validator existant:

1. Injecter `IInputSanitizationService`
2. Ajouter les validations de sécurité appropriées
3. Tester

**Exemple:**
```csharp
// AVANT
public class MyValidator : AbstractValidator<MyCommand>
{
    public MyValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}

// APRÈS
public class MyValidator : AbstractValidator<MyCommand>
{
    private readonly IInputSanitizationService _sanitizationService;

    public MyValidator(IInputSanitizationService sanitizationService)
    {
        _sanitizationService = sanitizationService;

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .SafeName(_sanitizationService);  // ✅ Ajouté
    }
}
```

## 🔐 Sécurité Supplémentaire

### Entity Framework protège déjà contre SQL Injection
L'utilisation de Entity Framework avec des requêtes paramétrées protège automatiquement contre l'injection SQL:

```csharp
// ✅ Sûr (paramétrisé)
await context.Users
    .Where(u => u.Email == email)
    .FirstOrDefaultAsync();

// ❌ Dangereux (ne jamais faire)
await context.Users
    .FromSqlRaw($"SELECT * FROM Users WHERE Email = '{email}'")
    .FirstOrDefaultAsync();
```

### Output Encoding
Le frontend doit également encoder les sorties. Mais la validation backend reste essentielle comme première ligne de défense.

## 📝 Checklist de Sécurité

Avant de déployer un nouveau Command/Query:

- [ ] Les champs texte utilisent `.SafeName()` ou `.SafeDescription()`
- [ ] Les noms de fichiers utilisent `.SafeFileName()`
- [ ] Les URLs utilisent `.SafeUrl()`
- [ ] Les validations ne sont pas trop restrictives (accents autorisés)
- [ ] Les messages d'erreur sont clairs
- [ ] Les tests couvrent les cas malveillants

## 🧪 Exemples de Tests

```csharp
[Fact]
public async Task Should_Reject_XSS_In_Name()
{
    // Arrange
    var command = new CreateUserCommand
    {
        FirstName = "<script>alert('xss')</script>",
        // ...
    };

    // Act
    var result = await _validator.ValidateAsync(command);

    // Assert
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.ErrorMessage == "ERR.Validation.ScriptTagsNotAllowed");
}

[Fact]
public async Task Should_Accept_Valid_Name_With_Accents()
{
    // Arrange
    var command = new CreateUserCommand
    {
        FirstName = "François",  // ✅ Doit être accepté
        // ...
    };

    // Act
    var result = await _validator.ValidateAsync(command);

    // Assert
    result.IsValid.Should().BeTrue();
}
```

## 🎓 Ressources Supplémentaires

- [OWASP XSS Prevention Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/Cross_Site_Scripting_Prevention_Cheat_Sheet.html)
- [OWASP SQL Injection Prevention](https://cheatsheetseries.owasp.org/cheatsheets/SQL_Injection_Prevention_Cheat_Sheet.html)
- [HtmlSanitizer Documentation](https://github.com/mganss/HtmlSanitizer)

## 📞 Support

Pour toute question sur la sécurité des entrées, contactez l'équipe de sécurité.
