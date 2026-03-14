# Interaction System - Tolga

> Ludu Arts Unity Developer Intern Case

## Proje Bilgileri

| Bilgi | Deger |
|-------|-------|
| Unity Versiyonu | 6000.0.68f1 (Unity 6) |
| Render Pipeline | URP (Universal Render Pipeline) |
| Case Suresi | 12 saat |
| Tamamlanma Orani | %0 (baslangic) |

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
| E (basili tut) | Hold etkilesim |

### Test Senaryolari

1. **Door Test:**
   - Kapiya yaklasin, "Press E to Open" mesajini gorun
   - E'ye basin, kapi acilsin
   - Tekrar basin, kapi kapansin

2. **Key + Locked Door Test:**
   - Kilitli kapiya yaklasin, "Locked - Key Required" mesajini gorun
   - Anahtari bulun ve toplayin
   - Kilitli kapiya geri donun, simdi acilabilir olmali

3. **Switch Test:**
   - Switch'e yaklasin ve aktive edin
   - Bagli nesnenin (kapi) tetiklendigini gorun

4. **Chest Test:**
   - Sandiga yaklasin
   - E'ye basili tutun, progress bar dolsun
   - Sandik acilsin ve icindeki item alinsin

---

## Mimari Kararlar

### Interaction System Yapisi

```
InteractionDetector (Player uzerinde)
    |
    |-- Raycast ile IInteractable tespit
    |-- En yakin IInteractable secimi
    |-- Input'a gore Interact() cagrisi
    |
IInteractable (Interface)
    |
    |-- InteractionType (Instant / Hold / Toggle)
    |-- Interact(), GetPromptMessage(), CanInteract()
    |
    +-- Door : MonoBehaviour, IInteractable (Toggle)
    +-- KeyPickup : MonoBehaviour, IInteractable (Instant)
    +-- Switch : MonoBehaviour, IInteractable (Toggle)
    +-- Chest : MonoBehaviour, IInteractable (Hold)

PlayerInventory (Player uzerinde)
    |
    |-- List<ItemData> - toplanan item'lar
    |-- HasKey(KeyType) kontrolu
    |
ItemData (ScriptableObject)
    |
    |-- Item tanimlari (isim, ikon, tur)
    |-- KeyItemData : ItemData (anahtar turu)

InteractionPromptUI (UI)
    |
    |-- Prompt mesaji gosterimi
    |-- Hold progress bar
    |-- Cannot interact feedback
```

**Neden bu yapiyi sectim:**
> Interface-based tasarim ile her interactable bagimsiz olarak implement edilebilir.
> Raycast-based detection, trigger-based'e gore daha hassas kontrol saglar ve
> oyuncunun bakis yonundeki nesneyi secmesi icin idealdir.
> ScriptableObject ile item tanimlari data-driven olarak yonetilebilir.

**Alternatifler:**
> - Trigger-based detection: Daha basit ama bakis yonunu dikkate almaz
> - Abstract base class: Interface yerine kullanilabilirdi ama MonoBehaviour inheritance zaten var
> - Event-driven detection: Performans icin iyi ama basit case icin overengineering

**Trade-off'lar:**
> - Raycast her frame calisir ama sadece tek ray oldugu icin performans maliyeti dusuk
> - Interface kullanmak explicit implementation gerektirir (Ludu Arts standardi)
> - ScriptableObject item sistemi basit ama genisletilebilir

### Kullanilan Design Patterns

| Pattern | Kullanim Yeri | Neden |
|---------|---------------|-------|
| Observer | Event system (OnInteracted, OnItemCollected) | Loose coupling |
| Strategy | InteractionType (Instant/Hold/Toggle) | Farkli etkilesim davranislari |
| Singleton | InteractionPromptUI | Tek UI instance |

---

## Ludu Arts Standartlarina Uyum

### C# Coding Conventions

| Kural | Uygulandi | Notlar |
|-------|-----------|--------|
| m_ prefix (private fields) | [x] | Tum private field'lar |
| s_ prefix (private static) | [x] | Static field'lar |
| k_ prefix (private const) | [x] | Constant degerler |
| Region kullanimi | [x] | Standart siralama |
| Region sirasi dogru | [x] | Fields > Events > Properties > Unity Methods > Methods > Interface Impl |
| XML documentation | [x] | Tum public API'ler |
| Silent bypass yok | [x] | Hatalar loglaniyor |
| Explicit interface impl. | [x] | IInteractable |

### Naming Convention

| Kural | Uygulandi | Ornekler |
|-------|-----------|----------|
| P_ prefix (Prefab) | [x] | P_Door, P_Chest, P_Switch, P_Key |
| M_ prefix (Material) | [x] | M_Door, M_Chest |
| SO isimlendirme | [x] | ItemData, KeyItemData |

### Prefab Kurallari

| Kural | Uygulandi | Notlar |
|-------|-----------|--------|
| Transform (0,0,0) | [x] | |
| Pivot bottom-center | [x] | |
| Collider tercihi | [x] | Box > Capsule > Mesh |
| Hierarchy yapisi | [x] | Root > Visual > Colliders |

---

## Tamamlanan Ozellikler

### Zorunlu (Must Have)

- [ ] Core Interaction System
  - [ ] IInteractable interface
  - [ ] InteractionDetector
  - [ ] Range kontrolu

- [ ] Interaction Types
  - [ ] Instant
  - [ ] Hold
  - [ ] Toggle

- [ ] Interactable Objects
  - [ ] Door (locked/unlocked)
  - [ ] Key Pickup
  - [ ] Switch/Lever
  - [ ] Chest/Container

- [ ] UI Feedback
  - [ ] Interaction prompt
  - [ ] Dynamic text
  - [ ] Hold progress bar
  - [ ] Cannot interact feedback

- [ ] Simple Inventory
  - [ ] Key toplama
  - [ ] UI listesi

### Bonus (Nice to Have)

- [ ] Animation entegrasyonu
- [ ] Sound effects
- [ ] Multiple keys / color-coded
- [ ] Interaction highlight
- [ ] Save/Load states
- [ ] Chained interactions

---

## Bilinen Limitasyonlar

### Tamamlanamayan Ozellikler
(Proje ilerledikce guncellenecek)

### Bilinen Bug'lar
(Test asamasinda guncellenecek)

### Iyilestirme Onerileri
(Proje sonunda guncellenecek)

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
│   │   │   │   └── InteractionData.cs
│   │   │   ├── Interactables/
│   │   │   │   ├── Door.cs
│   │   │   │   ├── KeyPickup.cs
│   │   │   │   ├── Switch.cs
│   │   │   │   └── Chest.cs
│   │   │   ├── Player/
│   │   │   │   ├── InteractionDetector.cs
│   │   │   │   └── PlayerInventory.cs
│   │   │   └── UI/
│   │   │       ├── InteractionPromptUI.cs
│   │   │       └── HoldProgressBarUI.cs
│   │   └── Editor/
│   ├── ScriptableObjects/
│   │   └── Items/
│   ├── Prefabs/
│   │   ├── Interactables/
│   │   ├── UI/
│   │   └── Player/
│   ├── Materials/
│   └── Scenes/
│       └── TestScene.unity
├── Docs/
│   ├── CSharp_Coding_Conventions.md
│   ├── Naming_Convention_Kilavuzu.md
│   └── Prefab_Asset_Kurallari.md
├── README.md
├── PROMPTS.md
└── .gitignore
```

---
## İletişim

| Bilgi | Değer |
|-------|-------|
| Ad Soyad | [Tolga Yıldız] |
| E-posta | [tolgayilddiz@gmail.com] |
| LinkedIn | [https://www.linkedin.com/in/tolgayilddiz/] |
| GitHub | [github.com/Togiylz] |

---
*Bu proje Ludu Arts Unity Developer Intern Case icin hazirlanmistir.*
