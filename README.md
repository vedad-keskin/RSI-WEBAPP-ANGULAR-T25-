# 🛠️ Fix for Long File Path Issues on Windows When Cloning This Repository

If you're trying to clone this repository and see an error like:

fatal: unable to checkout working tree
error: unable to create file [...]: Filename too long

This is a common issue on Windows due to the default 260-character path length limit.

---

## ✅ Solution: Enable Long Paths for Git on Windows

### Step 1: Open Command Prompt as Administrator

- Press `Start` and search for **Command Prompt**
- Right-click and choose **Run as administrator**
- Or press `Win + R`, type `cmd`, then press `Ctrl + Shift + Enter`

### Step 2: Run the following command

```bash
git config --system core.longpaths true

Now try cloning the repository again:

git clone https://github.com/vedad-keskin/Tutoring-codes.git
