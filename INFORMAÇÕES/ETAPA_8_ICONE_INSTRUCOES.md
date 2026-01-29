# ETAPA 8: Ícone do App - Instruções

## Situação Atual
- ✅ Ícone de origem baixado: `/workspaces/CELESTE/app_icon.jpg` (16x16px - muito pequeno)
- ⚠️ PIL/Pillow não disponível no container para processar mipmaps

## Solução Recomendada (Para Executar Localmente)

### Opção 1: Online (Rápido)
1. Abra http://romannurik.github.io/AndroidAssetStudio/icons-launcher.html
2. Upload `app_icon.jpg`
3. Download do zip com todos os mipmaps
4. Extrair em `src/Celeste.Android/Resources/`

### Opção 2: Localmente (Python + Pillow)
```bash
pip install Pillow
python3 create_mipmaps.py
```

### Opção 3: Manualmente (ImageMagick)
```bash
convert app_icon.jpg -resize 48x48 src/Celeste.Android/Resources/mipmap-mdpi/ic_launcher.png
convert app_icon.jpg -resize 72x72 src/Celeste.Android/Resources/mipmap-hdpi/ic_launcher.png
convert app_icon.jpg -resize 96x96 src/Celeste.Android/Resources/mipmap-xhdpi/ic_launcher.png
convert app_icon.jpg -resize 144x144 src/Celeste.Android/Resources/mipmap-xxhdpi/ic_launcher.png
convert app_icon.jpg -resize 192x192 src/Celeste.Android/Resources/mipmap-xxxhdpi/ic_launcher.png
```

## Estrutura Esperada
```
src/Celeste.Android/Resources/
├── mipmap-mdpi/
│   └── ic_launcher.png (48x48)
├── mipmap-hdpi/
│   └── ic_launcher.png (72x72)
├── mipmap-xhdpi/
│   └── ic_launcher.png (96x96)
├── mipmap-xxhdpi/
│   └── ic_launcher.png (144x144)
└── mipmap-xxxhdpi/
    └── ic_launcher.png (192x192)
```

## AndroidManifest.xml (Já Configurado)
O `android:icon="@mipmap/ic_launcher"` já está nas configurações padrão do template MonoGame.

## Status: PENDENTE
- ⏳ Aguardando execução local ou via Android Asset Studio online
- ✅ Estrutura dos Resources já existe em src/Celeste.Android/

## Bloqueadores
- Sem PIL/Pillow disponível no container
- Sem ImageMagick instalável (sem sudo)
- Sem acesso a X11 para aplicações gráficas

---

Próximo passo: Executar uma das 3 opções acima e recompile o APK.
