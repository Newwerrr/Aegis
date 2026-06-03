
# 🦋 Aegis

*A tiny Unity runtime bridge for mods and rendering things safely :3*

Aegis provides a lightweight hook layer inside Unity so mods can safely execute code on the Unity thread and attach to render/update events.

It does **not** create windows.  
It does **not** force a GUI system.  
It does **not** care what renderer or UI library you use.

Instead, Aegis acts as a small compatibility layer that allows things like:

- ImGui.NET
- custom overlays
- debug tools
- render callbacks
- Harmony-based mods
- thread-safe execution

to work inside Unity without every mod needing to reinvent the same plumbing.

---

## ✨ What Aegis does

Aegis creates:

- a persistent Unity runtime object
- a Unity-thread execution queue
- update hooks
- render hooks
- a simple API mods can subscribe to

Think of it like:

Game  
↓  
Harmony patches  
↓  
Aegis  
↓  
ImGui / overlays / custom tools  

---

## 📦 Installation

Add the Aegis assembly to your project/mod.

Initialize it once:

```csharp
AegisRuntime.Initialize();
````

done :3

---

## 🔧 Available API

```csharp
AegisRuntime.Initialize();

AegisRuntime.RunOnUnityThread(Action);

AegisRuntime.OnUpdate += YourMethod;

AegisRuntime.OnRender += YourMethod;
```

---

## 🧵 Unity thread execution

Unity gets very grumpy if you call certain APIs from random threads.

Aegis provides a queue that safely runs code on the Unity thread.

Example:

```csharp
Task.Run(() =>
{
    var data = DownloadStuff();

    AegisRuntime.RunOnUnityThread(() =>
    {
        player.Health = data.Health;
    });
});
```

Aegis handles moving the code back where Unity wants it.

---

## 🎨 Using ImGui.NET

Aegis does not render ImGui itself.

Instead, it provides a render hook so your renderer/backend can work correctly.

```csharp
using ImGuiNET;

AegisRuntime.OnRender += () =>
{
    ImGui.NewFrame();

    ImGui.Begin("hello");

    ImGui.Text("haiii :3");

    ImGui.End();

    ImGui.Render();
};
```

You are still responsible for your renderer backend.

Aegis only supplies the execution environment.

---

## 🔨 Harmony integration

Harmony works nicely with Aegis.

```csharp
using HarmonyLib;

[HarmonyPatch(typeof(Player), "TakeDamage")]
class DamagePatch
{
    static void Postfix(int amount)
    {
        AegisRuntime.RunOnUnityThread(() =>
        {
            DamageTracker.LastDamage = amount;
        });
    }
}
```

Now your patch can safely interact with Unity objects.

---

## ⚡ Update hooks

Subscribe to update events:

```csharp
AegisRuntime.OnUpdate += Tick;

void Tick()
{
    // runs every frame
}
```

---

## 🖥️ Render hooks

Subscribe to render events:

```csharp
AegisRuntime.OnRender += Draw;

void Draw()
{
    // render stuff here
}
```

---

## 💖 Philosophy

Aegis tries to stay tiny.

No giant framework.

No forced UI system.

No weird dependencies.

Just:

```text
hook thing
run thing
render thing
be happy
```

---

## 🌸 Planned things

* plugin system
* renderer adapters
* optional ImGui helpers
* mod registration API
* safer callback handling
* IL2CPP support
* more tiny creature energy

---

Made with caffeine and confusion 🦋
