# Last Signal

![Last Signal](Assets/ImageGame/Icon.jpg) <!-- Thay link này bằng ảnh minh họa của game -->

## 🧟‍♂️ Giới thiệu
**Last Signal** là một trò chơi sinh tồn trong thành phố Zombie góc nhìn thứ nhất, nơi bạn phải chiến đấu với lũ zombie để sống sót. Người chơi sẽ sử dụng các loại vũ khí như súng và kiếm để tiêu diệt zombie, thu thập tài nguyên, và cố gắng sống sót cho đến khi đội cứu hộ đến.

### Tính năng nổi bật:
- **Hệ thống vũ khí**: Súng tự động, súng bắn tỉa.
- **Hệ thống zombie thông minh**: Zombie có thể nghe thấy tiếng súng và tìm đến bạn.
- **Hệ thống lưu trữ**: Lưu và tải trạng thái trò chơi.
- **Cảnh báo âm thanh**: Zombie phản ứng với âm thanh từ súng của người chơi.


---

## 🚀 Cài đặt

### Yêu cầu hệ thống:
- **Unity**: Phiên bản 2021.3 hoặc mới hơn.
- **.NET Framework**: 4.7.1.
- **C#**: Phiên bản 9.0.

### Hướng dẫn cài đặt:
1. Clone dự án từ GitHub:
2. Mở dự án trong Unity.
3. Đảm bảo tất cả các asset và package đã được tải đầy đủ.
4. Nhấn **Play** trong Unity Editor để bắt đầu chơi.

---

## 🎮 Hướng dẫn chơi

### Điều khiển cơ bản:
| Phím | Hành động                  |
|------|----------------------------|
| W/A/S/D | Di chuyển                |
| Chuột trái | Bắn súng |
| Chuột phải | Ngắm bắn |
| Q | Tấn công cận chiến           |
| R | Nạp đạn                      |
| Shift | Chạy nhanh                |
| Space | Nhảy                     |


### Mục tiêu:
- Sống sót trước lũ zombie.
- Thu thập tài nguyên và đạn dược.
- Chờ đội cứu hộ đến trong thời gian quy định.

---

## 🛠️ Cấu trúc dự án

### **Scripts**
- **Player**:
  - `GunScript.cs`: Quản lý súng, bắn đạn, và nạp đạn.
  - `PlayerController.cs`: Quản lý trạng thái người chơi (máu, đạn, v.v.).
  - `PlayerMovementScript.cs`: Xử lý di chuyển và tương tác vật lý của người chơi.
  - `PlayerHealth.cs`: Quản lý máu của người chơi.
- **Enemy**:
  - `AttackState.cs`: Trạng thái tấn công của zombie.
  - `SearchState.cs`: Trạng thái tìm kiếm của zombie.
  - `ZombiePool.cs`: Hệ thống quản lý pool zombie.
- **Storage**:
  - `SaveSystem.cs`: Lưu và tải trạng thái trò chơi.
- **Sound**:
  - `BackgroudSoundManager.cs`: Quản lý âm thanh nền.
- **UI**:
  - `PlayerUI.cs`: Hiển thị các thông số như máu và đạn của người chơi.
  - `UIMenuManager.cs`: Hiển thị Menu tùy chỉnh các thông số như âm thanh, đồ họa.

### **Prefabs**
- **Zombie**: Các loại zombie khác nhau.
- **Weapons**: Súng.
- **Items**: Đạn và máu.

### **Scenes**
- `MainMenu`: Menu chính của trò chơi.
- `GameScene`: Cảnh chính nơi người chơi chiến đấu với zombie.

---

## 📖 Hệ thống chính

### 1. **Hệ thống vũ khí**
- **GunScript.cs**:
  - Hỗ trợ hai loại súng: tự động và không tự động.
  - Lưu trạng thái đạn bằng `PlayerPrefs`.
  - Tích hợp hiệu ứng âm thanh và hình ảnh khi bắn.

### 2. **Hệ thống zombie**
- Zombie có thể:
  - Nghe thấy tiếng súng và tìm đến người chơi.
  - Tấn công khi ở gần người chơi.
  - Chuyển đổi trạng thái giữa "Tìm kiếm" và "Tấn công".

### 3. **Hệ thống lưu trữ**
- Lưu trạng thái trò chơi (máu, đạn, vị trí) bằng `SaveSystem.cs`.
- Tải lại trạng thái khi người chơi chọn "Continue Game".

### 4. **Hệ thống âm thanh**
- Quản lý âm thanh nền và âm thanh hành động.
- Zombie phản ứng với âm thanh từ súng.

---

## 📷 Hình ảnh minh họa
<!-- Thêm ảnh chụp màn hình của game -->
1. **Menu chính**:
   ![Main Menu](Assets/ImageGame/Startgame_Play.png)
   ![Main Menu](Assets/ImageGame/Startgame_Extras.png)
   ![Main Menu](Assets/ImageGame/StartGame_Setting.png)

2. **Chiến đấu với zombie**:
   ![Gameplay](Assets/ImageGame/PlayGame_Start.png)
   ![Gameplay](Assets/ImageGame/PlayGame_Zombie.png)



---

## 🛡️ Bản quyền
Dự án này được phát triển bởi **[Nguyễn Thành Nghiêm]**. Mọi quyền được bảo lưu.

---

## 🤝 Đóng góp
Nếu bạn muốn đóng góp cho dự án, hãy làm theo các bước sau:
1. Fork dự án.
2. Tạo một nhánh mới:
3. Commit thay đổi của bạn:
4. Push nhánh của bạn:
5. Tạo một Pull Request trên GitHub.

---

## 📧 Liên hệ
Nếu bạn có bất kỳ câu hỏi hoặc góp ý nào, vui lòng liên hệ qua email: **thanhnghiem199@gmail.com**.

---

## 🌟 Cảm ơn
Cảm ơn bạn đã chơi **Last Signal**! Chúc bạn có những giây phút vui vẻ và hồi hộp khi chiến đấu với lũ zombie!
