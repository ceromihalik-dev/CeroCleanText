# CeroCleanText 0.1.0 – Release QA

## Build baseline

- Branch: develop
- Target: CeroCleanText 0.1.0
- Windows: x64
- Packages: Standard / Portable

## Already physically verified

- [x] Tray application starts
- [x] Selected-text cleanup via global hotkey
- [x] Selected-text cleanup via tray menu
- [x] Unicode cleanup test including zero-width characters and special whitespace
- [x] German characters Ä Ö Ü ä ö ü ß and € preserved
- [x] Configurable global hotkey
- [x] Hotkey persists after application restart
- [x] C/broom branding visible in EXE, tray and dialogs
- [x] Settings and About dialogs open

## Release QA – remaining

### Startup and process
- [x] Autostart enabled
- [x] Windows restart -> CeroCleanText starts automatically
- [x] Tray icon available after restart
- [x] Only one CeroCleanText instance can run

### Notification
- [x] Cleanup notification enabled -> shown after changed text
- [x] Clean text / no change -> no success notification
- [x] Notification disabled -> no notification after changed text
- [x] Notification preference persists after restart

### Clipboard
- [x] Clean clipboard command works
- [x] Selected-text cleanup restores previous clipboard content – regression fixed and physically retested PASS

### External links
- [x] GitHub link opens correct project
- [x] PayPal support link opens correct hosted donation page

### Compatibility
- [x] Notepad
- [x] Microsoft Word
- [x] Browser text field
- [x] Optional: Thunderbird / Outlook

### Resource usage
- [x] Idle CPU checked – 0 % observed
- [x] Idle memory checked – 9.1 MB initial idle observed; stress plateau 23.9 MB -> 23.8 MB after repeated dialog/cleanup cycles
- [x] No unexpected background polling observed – idle disk 0 MB/s and network 0 Mbit/s observed; repeated stress cycle shows no continuing memory growth

### Packaging
- [ ] Standard build starts with .NET 10 Desktop Runtime installed
- [ ] Portable build starts without separate runtime dependency
- [ ] Version metadata = 0.1.0
- [ ] C/broom icon visible on release executable
- [ ] Windows security/SmartScreen behavior documented

## Release gate

Status: IN QA – startup/process/notification, clipboard/link, compatibility and resource blocks PASS

Release only after all required checks pass or any accepted limitation is explicitly documented.
