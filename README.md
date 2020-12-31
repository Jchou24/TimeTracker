# Time Tracker

簡易工時填寫系統 + 含簡易授權機制之會員系統

* Web Socket 工時填寫即時連動機制

* 工時資料彙整報表

* 表單選項調整機制

* 使用者帳號授權機制

* 桌面瀏覽器簡易RWD

* 跨Tab多段閒置登出機制

  * 多Tab同步閒置登出

  * 閒置後會出現詢問USER是否繼續使用

* 跨平台，可在 windows 或 linux 上部署

# 開發使用技術

## 前端

* Vue2 SPA(vuex + vue-router)

* Typescript

* Composition API

* Vuetify

## 後端

* .Net Core 3.1

* .Net Core Entity Framework(code first)

## DB

* MS SQL

* Postgres SQL

# Online Demo

* [Demo 網站](https://time-tracker-vue-netcore.herokuapp.com/)

* 網站使用 Heroku 部屬，閒置啟動可能會需要花一點時間

* 預設帳號/密碼

  * Admin

    * admin@auth.com

    * test

  * User

    * user@auth.com

    * test                

# Installation

## 1. Windows + MSSql Prebuild

可以使用 \release_windows 中之預編譯程式

### DB

在 SSMS 執行 CreateDB_CreateTable_CreateInitData.sql 直接

1. 建立 DB

2. 建立 Tables

3. 建立初始資料

### 部屬網站

解壓縮 publish.zip

在 IIS 設定路徑時使用 publish 資料夾的檔案即可

## 2. Build

1. 到 ClientApp 中用 `npm run build` 編譯前端

2. 利用 Visual Studio 功能或 .net core cli 指令編譯程式成 windows 程式或 linux 程式

3. DB Code First Migration

```bash
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

# Config

* IsLaunchSPAServer: `true`, `false`

  * Debug Mode 自動啟動SPA Server

* DBType: `MS`, `PG`

  * Environment Variable:

    * `DBTYPE`: 當環境變數設定此值時，將優先從此環境變數讀取 DBType

  * `MS`: 使用 MS SQL

  * `PG`: 使用 Postgres SQL

* ConnectionStrings

  * Environment Variable:

    * `TIMETRACKER_CONNECTIONNSTRING`: 當環境變數設定此值時，將優先從此環境變數讀取 connection string

  * `MS`: 當 `TIMETRACKER_CONNECTIONNSTRING` 未被設定 & DBType 為 `MS`，將從此值讀取 connection string

  * `PG`: 當 `TIMETRACKER_CONNECTIONNSTRING` 未被設定 & DBType 為 `PG`，將從此值讀取 connection string

# Features

## 權限管理

* 授權機制

  * 目前區分 `Admin`, `User`

  * `Admin`

    * 可以管理權限

    * 能看到 `系統設定頁`，`權限管理頁`

    * 看不到 `工時填寫頁`，`工時報表頁`，`全體工時報表`

  * `User`

    * 能進行工時的填寫

    * 能看到 `工時填寫頁`，`工時報表頁`，`全體工時報表`

    * 看不到 `系統設定頁`，`權限管理頁`

## 權限管理頁

* 開通新帳號(Uncheck Accounts)

  * 剛申請的帳號需要admin開通

* 調整帳號(Accounts)

  * 權限

  * 狀態

    * 啟用(Approved)

    * 停用(Reject)

    * 暫停使用(Suspend)

    * 待確認(Uncheck)
                    
## 系統設定頁

* CRUD 週期

* CRUD 工作類型

* CRUD 工作來源

## 工時填寫

* 左上角可以選擇

  * 要填寫誰的工時(只能選擇1人)

  * 要填寫哪個區間的工時
    
    選擇方法有兩種

      1. 選擇週期，由週期帶入時間區間

      2. 或者也可以直接選擇時間區間

* 填寫工時報表可以多人同時協作，資料會即時同步

  但當右上角出現連線中斷的黃色提示時，需要重新F5瀏覽器

## 工時報表

* 使用左上角的選擇器，調整對象跟時間，可以觀看"單人"的工時彙整

* 資料不會即時同步

## 全體工時報表

* 使用左上角的選擇器，調整對象跟時間，可以觀看"多人"的工時彙整

* 預設會選擇全部帳號"啟用中"的人

* 資料不會即時同步
