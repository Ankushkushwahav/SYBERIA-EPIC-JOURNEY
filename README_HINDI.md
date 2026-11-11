# SYBERIA: EPIC JOURNEY — Unity 6 Starter Project

यह एक original, playable 3D adventure starter project है। इसमें 8 original chapters के लिए foundation, third-person movement, follow camera, keyboard + on-screen mobile controls, interaction, mission, inventory, save/load और runtime-generated Bazaar environment शामिल हैं।

## जरूरी जानकारी
- Unity 6 (6000.x) में नया **Universal 3D (URP)** project बनाएँ।
- ZIP extract करें और इसके `Assets` folder को Unity project के `Assets` folder में merge/copy करें।
- Unity में compile पूरा होने दें।
- Menu: **SYBERIA > Create Demo Scene** चुनें।
- `Assets/Scenes/VaghenBazaar_Demo.unity` खुल जाएगी।
- Play दबाएँ।

## Controls
- PC: WASD / Arrow keys = move, Shift = run, Space = jump, E = interact, I = inventory, Esc = pause.
- Android: स्क्रीन के बाएँ हिस्से में virtual joystick; दाएँ buttons Move/Jump/Interact/Inventory. Right side drag camera rotation.
- Keyboard और touch दोनों supported हैं।

## Android / Windows / WebGL
1. Unity Hub > Installs > Unity 6 > Add modules: Android Build Support (SDK/NDK/OpenJDK), Windows Build Support, WebGL Build Support.
2. File > Build Profiles में target चुनें।
3. Android: Switch Platform, Player Settings में package name सेट करें, फिर Build.
4. Windows: Windows target चुनकर Build.
5. WebGL: Web target चुनकर Build. WebGL में local file save browser restrictions के कारण अलग व्यवहार कर सकता है; इस starter में PlayerPrefs save है।

## Included
- Runtime scene generator (stylized original old-European bazaar, plaza, buildings, lamps, fountain, train prop)
- Third-person CharacterController movement
- Camera orbit/follow
- Touch joystick + action buttons
- Interactable lost mechanical key
- Mission progress + HUD
- Inventory
- Save/load with PlayerPrefs
- Eight chapter data entries (chapters 2–8 unlock sequentially after completing prior chapter)

## Notes
यह polished AAA production नहीं है; यह complete source-code starter/demo है, जिसे आगे custom 3D models, animations, voice acting, music, cutscenes और detailed puzzle content से विस्तार देना होगा। किसी commercial Syberia game की proprietary assets, dialogue, music या exact level design शामिल नहीं हैं। All names, locations and story content here are original-inspired placeholder content.
