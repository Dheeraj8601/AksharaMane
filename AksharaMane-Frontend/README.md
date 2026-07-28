# AksharaMane Frontend

Simple bookstore frontend built with React, Vite, Bootstrap, external CSS, Axios, and React Router.

## Customer scope
- Home
- Categories
- Book list
- Book details
- Buy one book by entering delivery address
- Cash on Delivery only
- No customer login, cart, wishlist, or online payment

## Admin scope
- Admin login only
- Dashboard
- Category CRUD
- Book CRUD
- Order list
- Change order status
- Backend notification endpoint is called after status update

## Run

```bash
npm install
copy .env.example .env
npm run dev
```

On macOS/Linux:

```bash
cp .env.example .env
```

By default:

```env
VITE_USE_MOCK_API=true
```

The complete UI works using localStorage mock data.

When the ASP.NET Core backend is ready:

```env
VITE_USE_MOCK_API=false
VITE_API_BASE_URL=https://localhost:7120/api
```

## Demo admin login

- Email: `admin@aksharamane.com`
- Password: `Admin@123`
