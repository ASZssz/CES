ssh codespace@135.237.130.226# INSTRUÇÕES PARA CONTINUAR O PROJETO

## Status Atual

O projeto Celeste/MonoGame Android está em **FASE DE ESTRUTURA PRONTA**:

- ✅ Solution configurada com 3 projetos (Core, Desktop, Android)
- ✅ Projetos compilam com sucesso
- ✅ LogSystem funcional
- ✅ Serviços de plataforma (Desktop/Android) preparados
- ✅ Content folders copiados
- ✅ Android configurado para arm64-v8a only

## Próximas Etapas

### 1. Copiar Código Original

Copie gradualmente o código decompilado:

```bash
cd /workspaces/CELESTE/src

# Monocle (engine base)
cp -r ../Monocle Celeste.Core/

# Celeste (game logic)
cp -r ../Celeste Celeste.Core/

# SimplexNoise (utilities)
cp -r ../SimplexNoise Celeste.Core/

# FMOD (audio bindings)
cp -r ../FMOD Celeste.Core/
cp -r ../FMOD.Studio Celeste.Core/

# Editor (Desktop-only, opcional)
cp -r ../Celeste.Editor Celeste.Core/

# Pico8 (Platform, opcional)
cp -r ../Celeste.Pico8 Celeste.Core/
```

### 2. Compilar e Listar Erros

```bash
cd /workspaces/CELESTE/src
dotnet build Celeste.Core -c Debug 2>&1 | grep "error CS" | head -50
```

Isso vai listar os ~95 erros de incompatibilidade XNA/MonoGame.

### 3. Estratégia para Resolver Erros

Para cada classe com erro:

**Opção A - Se API difference simples (Vector2, SpriteBatch, etc):**
```csharp
// Comentar método/linha temporariamente
// TODO: Fix for MonoGame compatibility
// problematic_code();
```

**Opção B - Se namespace não existe:**
```csharp
#if MONOGAME
    // MonoGame version
#else
    // XNA version
#endif
```

**Opção C - Criar wrapper:**
```csharp
// Em Celeste.Core/Wrappers/
namespace Monocle
{
    public static class MtextureCompat
    {
        public static void DrawMonogame(MTexture tex, Vector2 pos) { /* ... */ }
    }
}
```

### 4. Teste Desktop Primeiro

Uma vez que Core compile:

```bash
# Test Desktop
cd /workspaces/CELESTE/src
dotnet run --project Celeste.Desktop -c Debug

# Should see:
# === CELESTE DESKTOP START ===
# Game1.Initialize() called
# Game1.LoadContent() called
# [Black window should appear]
```

### 5. Depois Android

```bash
# Build APK
dotnet build Celeste.Android -c Release -p:AndroidSdkDirectory=/home/codespace/android-sdk

# Publish
dotnet publish Celeste.Android -c Release -p:AndroidSdkDirectory=/home/codespace/android-sdk

# APK will be at:
# src/Celeste.Android/bin/Release/net9.0-android/publish/Celestegame.app-Signed.apk
```

## Arquivos-Chave a Saber

### Core (Lógica Compartilhada)
- `Services.cs` - Interfaces de plataforma
- `Stubs/Namespaces.cs` - Placeholders (remover quando copiar real)

### Desktop
- `Program.cs` - Entrypoint (inicializa serviços)
- `DesktopServices.cs` - Implementações Desktop
- `Game1.cs` - Classe do jogo

### Android
- `Activity1.cs` - Entrypoint Android (Activity)
- `AndroidServices.cs` - Implementações Android
- `Game1.cs` - Classe do jogo
- `AndroidManifest.xml` - Configuração (package name, permissões)

## Configurações Importante

### Package Name
Já configurado como: `Celestegame.app`

Localização: `/src/Celeste.Android/Celeste.Android.csproj` (ApplicationId)

### Android ABIs
Configurado para arm64-v8a only:

```xml
<RuntimeIdentifiers>android-arm64</RuntimeIdentifiers>
```

### Frameworks
- Core: `net8.0`
- Desktop: `net8.0` (mgdesktopgl)
- Android: `net9.0-android` (mgandroid, min API 21)

## Logs

Logs são salvos em:
- **Desktop:** `./Logs/session_YYYY-MM-DD.log`
- **Android:** `/data/data/Celestegame.app/files/Logs/session_YYYY-MM-DD.log`

Crashes:
- **Desktop:** `./Logs/crash_YYYY-MM-DD_HH-mm-ss.log`
- **Android:** `/data/data/Celestegame.app/files/Logs/crash_YYYY-MM-DD_HH-mm-ss.log`

Ver logs do Android:
```bash
adb logcat -s CELESTE
adb pull /data/data/Celestegame.app/files/Logs/ ./
```

## Dependências

Já instaladas via NuGet:
- MonoGame.Framework.DesktopGL 3.8.*
- MonoGame.Framework.Android 3.8.*
- MonoGame.Content.Builder.Task 3.8.*

## Content Pipeline

Content não está sendo compilado via MGCB ainda. Próximas etapas:
1. Criar `Content/Content.mgcb`
2. Integrar no build
3. Configurar assets raw (FMOD banks, maps.bin, etc)

## Checklist de Conclusão

Quando tudo funcionar:
- [ ] Core compila sem erros
- [ ] Desktop inicia e roda
- [ ] Android inicia e roda (via emulator ou device)
- [ ] Input funciona (teclado, mouse, gamepad)
- [ ] Áudio funciona (FMOD)
- [ ] Menu principal carrega
- [ ] Gameplay básico funciona
- [ ] Fullscreen no Android
- [ ] FPS counter funciona
- [ ] Logs salvam corretamente
- [ ] APK gera sem erros

## Contato / Dúvidas

Veja RELATORIO.md para mais detalhes técnicos e decisões documentadas.

---

**Criado:** 28/01/2026
**Último update:** 28/01/2026 22:30 UTC
