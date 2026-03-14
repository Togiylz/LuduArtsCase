# Interaction System - Tolga

> Ludu Arts Unity Developer Intern Case

## Proje Bilgileri

| Bilgi | Deger |
|-------|-------|
| Unity Versiyonu | 6000.0.68f1 (Unity 6) |
| Render Pipeline | URP (Universal Render Pipeline) |
| Input System | New Input System (com.unity.inputsystem 1.18.0) |

---

## Kurulum

1. Repository'yi klonlayin:
```bash
git clone https://github.com/[username]/LuduArtsCase.git
```

2. Unity Hub'da projeyi acin (Unity 6000.0.68f1)
3. `Assets/InteractionSystem/Scenes/TestScene.unity` sahnesini acin
4. Play tusuna basin

---

## Nasil Test Edilir

### Kontroller

| Tus | Aksiyon |
|-----|---------|
| WASD | Hareket |
| Mouse | Bakis yonu |
| E | Etkilesim (Instant / Toggle) |
| E (basili tut) | Hold etkilesim (Chest) |
| Tab | Envanter ac/kapa |

### Test Senaryolari

1. **Door Test:**
   - Kapiya yaklasin, "Press E to Open" mesajini gorun
   - E'ye basin, kapi menteseden acilsin
   - Tekrar basin, kapi kapansin

2. **Key + Locked Door Test:**
   - Kilitli kapiya yaklasin, kirmizi "Locked - Red Key Required" mesajini gorun
   - Anahtari bulun ve E ile toplayin
   - Tab ile envanterde gorundugundan emin olun (ikon + isim)
   - Kilitli kapiya geri donun, simdi acilabilir

3. **Switch -> Door Test:**
   - Switch'e yaklasin ve E ile aktive edin
   - Bagli kapinin otomatik acildigini gorun
   - Tekrar E ile deaktive edin, kapi kapansin

4. **Chest Test:**
   - Sandiga yaklasin, "Hold E to Open (2s)" mesajini gorun
   - E'ye basili tutun, slider dolsun
   - Sandik acilsin ve icindeki item envantere eklensin
   - Tekrar deneyince "Already Opened" mesaji gorun

5. **Out of Range Test:**
   - Uzaktan bir nesneye bakin, "Too Far" mesajini gorun
   - Yaklastikca normal prompt'a gectigini gorun

---

## Mimari Kararlar

### Interaction System Yapisi

```
InteractionDetector (Player - SphereCast)
    |-- IInteractable tespit (menzil ici + menzil disi)
    |-- Input handling (Instant/Hold/Toggle)
    |-- Event-driven UI bildirimi
    |
IInteractable (Interface - explicit impl.)
    |-- Door     : Toggle, locked/unlocked, key kontrolu
    |-- KeyPickup: Instant, envantere ekleme
    |-- Switch   : Toggle, UnityEvent baglanti
    |-- Chest    : Hold, tek kullanimlik

PlayerInventory (ScriptableObject tabanli)
    |-- ItemData / KeyItemData
    |-- HasKeyOfType(), UseKey()

Bonus Sistemler:
    |-- InteractionHighlight  : Emission pulse efekti
    |-- InteractionSoundPlayer: ScriptableObject ses profilleri
    |-- InteractionSaveSystem : JSON + PlayerPrefs
```

**Neden bu yapiyi sectim:**
- Interface-based tasarim: Her interactable bagimsiz, SOLID uyumlu
- SphereCast: Raycast'tan daha toleransli, FPS icin ideal
- Explicit interface impl.: Ludu Arts standardi, encapsulation
- ScriptableObject item/sound: Data-driven, Inspector-friendly
- Event-driven UI: Loose coupling, performansli

**Trade-off'lar:**
- SphereCast her frame calisir ama tek ray, dusuk maliyet
- MaterialPropertyBlock ile highlight: GC allocation yok, performansli
- PlayerPrefs save: Basit ama buyuk veriler icin uygun degil

### Kullanilan Design Patterns

| Pattern | Kullanim Yeri | Neden |
|---------|---------------|-------|
| Observer | Event system (OnTargetChanged, OnItemAdded vb.) | Loose coupling |
| Strategy | InteractionType (Instant/Hold/Toggle) | Farkli davranislar |
| Singleton | InteractionSaveSystem | Global erisim |
| Component | Highlight, SoundPlayer, SaveSystem | Modularite |

---

## Ludu Arts Standartlarina Uyum

### C# Coding Conventions

| Kural | Uygulandi | Notlar |
|-------|-----------|--------|
| m_ prefix (private fields) | [x] | Tum private field'lar |
| s_ prefix (private static) | [x] | InteractionSaveSystem.s_Instance |
| k_ prefix (private const) | [x] | k_SphereCastRadius, k_RotationSpeed vb. |
| Region kullanimi | [x] | Fields > Events > Properties > Unity Methods > Methods > Interface Impl |
| Region sirasi dogru | [x] | Standart siralama |
| XML documentation | [x] | Tum public API'ler |
| Silent bypass yok | [x] | Debug.LogError/LogWarning ile loglama |
| Explicit interface impl. | [x] | IInteractable tum nesnelerde |

### Naming Convention

| Kural | Uygulandi | Ornekler |
|-------|-----------|----------|
| P_ prefix (Prefab) | [x] | P_Door, P_Chest, P_Switch, P_Key |
| M_ prefix (Material) | [x] | M_Door, M_Key_Red, M_Key_Blue |
| SO isimlendirme | [x] | ItemData, KeyItemData, InteractionSoundData |

### Prefab Kurallari

| Kural | Uygulandi | Notlar |
|-------|-----------|--------|
| Transform (0,0,0) | [x] | Tum prefab'lar |
| Pivot dogru | [x] | Kapi mentesede, sandik kapak arkada |
| Collider tercihi | [x] | Box Collider kullanildi |
| Hierarchy yapisi | [x] | Root > Pivot > Visual |

---

## Tamamlanan Ozellikler

### Zorunlu (Must Have)

- [x] Core Interaction System
  - [x] IInteractable interface
  - [x] InteractionDetector (SphereCast)
  - [x] Range kontrolu + Out of Range feedback

- [x] Interaction Types
  - [x] Instant
  - [x] Hold
  - [x] Toggle

- [x] Interactable Objects
  - [x] Door (locked/unlocked, key ile acma)
  - [x] Key Pickup (2 tip: Red, Blue)
  - [x] Switch/Lever (UnityEvent ile kapi baglantisi)
  - [x] Chest/Container (2sn hold, tek kullanimlik)

- [x] UI Feedback
  - [x] Interaction prompt (dinamik text)
  - [x] Hold progress bar (Slider)
  - [x] Out of Range feedback
  - [x] Cannot interact feedback (kirmizi text)

- [x] Simple Inventory
  - [x] Key toplama ve saklama
  - [x] UI listesi (ikon + isim)
  - [x] ScriptableObject item tanimlari

### Bonus (Nice to Have)

- [x] Animation entegrasyonu (+3) - Kapi rotasyon, sandik kapak, lever hareket
- [x] Sound effects integration (+2) - ScriptableObject tabanli ses profilleri
- [x] Multiple keys / color-coded (+2) - Red, Blue, Gold key turleri
- [x] Interaction highlight (+3) - Emission pulse efekti
- [x] Save/Load states (+3) - JSON + PlayerPrefs
- [x] Chained interactions (+2) - Switch -> Door UnityEvent baglantisi

---

## Dosya Yapisi

```
Assets/
├── InteractionSystem/
│   ├── Scripts/
│   │   ├── Runtime/
│   │   │   ├── Core/
│   │   │   │   ├── IInteractable.cs
│   │   │   │   ├── InteractionType.cs
│   │   │   │   ├── KeyType.cs
│   │   │   │   ├── ItemData.cs
│   │   │   │   ├── KeyItemData.cs
│   │   │   │   ├── InteractionHighlight.cs
│   │   │   │   ├── InteractionSoundData.cs
│   │   │   │   ├── InteractionSoundPlayer.cs
│   │   │   │   └── InteractionSaveSystem.cs
│   │   │   ├── Interactables/
│   │   │   │   ├── Door.cs
│   │   │   │   ├── KeyPickup.cs
│   │   │   │   ├── Switch.cs
│   │   │   │   └── Chest.cs
│   │   │   ├── Player/
│   │   │   │   ├── SimplePlayerController.cs
│   │   │   │   ├── InteractionDetector.cs
│   │   │   │   └── PlayerInventory.cs
│   │   │   └── UI/
│   │   │       ├── InteractionPromptUI.cs
│   │   │       ├── HoldProgressBarUI.cs
│   │   │       ├── InventoryUI.cs
│   │   │       └── InventorySlotUI.cs
│   │   └── Editor/
│   ├── ScriptableObjects/Items/
│   ├── Prefabs/
│   │   ├── Interactables/
│   │   ├── UI/
│   │   └── Player/
│   ├── Materials/
│   └── Scenes/
│       └── TestScene.unity
Docs/
├── CSharp_Coding_Conventions.md
├── Naming_Convention_Kilavuzu.md
└── Prefab_Asset_Kurallari.md
README.md
PROMPTS.md
.gitignore
```

---

## Bilinen Limitasyonlar

### Iyilestirme Onerileri
- Save sistemi PlayerPrefs yerine dosya tabanli olabilir
- Envanter sistemi stack/miktar destegi eklenebilir
- Object pooling ile pickup nesneleri optimize edilebilir
- Highlight icin outline shader daha iyi gorsel sonuc verir

---

*Bu proje Ludu Arts Unity Developer Intern Case icin hazirlanmistir.*
