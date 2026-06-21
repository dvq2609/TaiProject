/* =============================================
   VƯƠN - Common JavaScript
   Mock data, state management, shared components
   ============================================= */

// ── Mock Data ──────────────────────────────────
const PRODUCTS = [
  { id: '1', name: 'Rau cải xanh', category: 'vegetable', price: 120000, image: 'https://tse1.mm.bing.net/th/id/OIP.YMyyO4B2E7JowCCN-rgPEwHaFj?rs=1&pid=ImgDetMain&o=7&rm=3', description: 'Rau cải xanh tươi, dễ trồng, phù hợp cho người mới bắt đầu', difficulty: 'Dễ', light: 'Nhiều ánh sáng', careLevel: 'Tưới nước 2 lần/ngày', rating: 4.5, reviews: 128, inStock: true, stock: 50 },
  { id: '2', name: 'Cà chua bi', category: 'vegetable', price: 120000, image: 'https://images.unsplash.com/photo-1592841200221-a6898f307baa?w=400', description: 'Cà chua bi ngọt, năng suất cao', difficulty: 'Trung bình', light: 'Nhiều ánh sáng', careLevel: 'Tưới nước 1 lần/ngày, bón phân định kỳ', rating: 4.8, reviews: 95, inStock: true, stock: 30 },
  { id: '3', name: 'Hoa hồng', category: 'flower', price: 120000, image: 'https://img.thuthuatphanmem.vn/uploads/2018/09/24/hinh-anh-hoa-hong-dep-nhat_053955504.jpg', description: 'Hoa hồng đỏ thắm, thơm ngát', difficulty: 'Khó', light: 'Nhiều ánh sáng', careLevel: 'Cần chăm sóc kỹ lưỡng', rating: 4.9, reviews: 203, inStock: true, stock: 20 },
  { id: '4', name: 'Hoa oải hương', category: 'flower', price: 120000, image: 'https://charsawfarms.com/cdn/shop/files/PurpleBouquetlavender2.jpg?v=1710207668&width=1946', description: 'Hoa oải hương tím, hương thơm dễ chịu', difficulty: 'Dễ', light: 'Nhiều ánh sáng', careLevel: 'Ít cần chăm sóc', rating: 4.6, reviews: 87, inStock: true, stock: 45 },
  { id: '5', name: 'Combo cây rau gia vị', category: 'combo', price: 120000, image: 'https://images.unsplash.com/photo-1466692476868-aef1dfb1e735?w=400', description: 'Bộ 5 loại rau gia vị: húng quế, rau mùi, ngò gai, rau húng, tía tô', difficulty: 'Dễ', light: 'Ánh sáng trung bình', careLevel: 'Tưới nước đều đặn', rating: 4.7, reviews: 156, inStock: true, stock: 25 },
  { id: '6', name: 'Phân bón hữu cơ', category: 'accessory', price: 10000, image: 'https://th.bing.com/th/id/R.6c286dba498a4c368b9da7b62e2e04a6?rik=HGOrgHSmda8Yqg&pid=ImgRaw&r=0', description: 'Phân bón hữu cơ cho cây trồng', difficulty: 'Dễ', light: 'Ít ánh sáng', careLevel: 'Không cần chăm sóc', rating: 4.4, reviews: 67, inStock: true, stock: 100 },
  { id: '7', name: 'Rau diếp xanh', category: 'vegetable', price: 120000, image: 'https://images.unsplash.com/photo-1622206151226-18ca2c9ab4a1?w=400', description: 'Rau diếp tươi ngon, giàu vitamin', difficulty: 'Dễ', light: 'Ánh sáng trung bình', careLevel: 'Tưới nước 2 lần/ngày', rating: 4.3, reviews: 45, inStock: true, stock: 60 },
  { id: '8', name: 'Hoa cúc vàng', category: 'flower', price: 120000, image: 'https://media.chuabavang.com/files/tu_chinh/2021/12/28/hoa-cuc-vang-clb-cuc-vang-chua-ba-vang-0839.jpg', description: 'Hoa cúc vàng rực rỡ, dễ trồng', difficulty: 'Dễ', light: 'Nhiều ánh sáng', careLevel: 'Tưới nước vừa phải', rating: 4.5, reviews: 92, inStock: true, stock: 35 },
  { id: '9', name: 'Rau húng quế', category: 'vegetable', price: 80000, image: 'https://images.unsplash.com/photo-1618375569909-3c8616cf7733?w=400', description: 'Húng quế thơm, dễ trồng trong chậu', difficulty: 'Dễ', light: 'Ánh sáng trung bình', careLevel: 'Tưới 1 lần/ngày', rating: 4.6, reviews: 78, inStock: true, stock: 80 },
  { id: '10', name: 'Chậu đất nung', category: 'accessory', price: 45000, image: 'https://images.unsplash.com/photo-1622372738946-62e02505feb3?w=400', description: 'Chậu đất nung thoáng khí, giúp rễ cây phát triển tốt', difficulty: 'Dễ', light: 'Ít ánh sáng', careLevel: 'Không cần chăm sóc', rating: 4.2, reviews: 34, inStock: true, stock: 150 },
  { id: '11', name: 'Combo kit trồng rau', category: 'combo', price: 250000, image: 'https://images.unsplash.com/photo-1416879595882-3373a0480b5b?w=400', description: 'Bộ kit đầy đủ: chậu + đất + hạt giống + hướng dẫn', difficulty: 'Dễ', light: 'Ánh sáng trung bình', careLevel: 'Theo hướng dẫn kèm theo', rating: 4.9, reviews: 312, inStock: true, stock: 40 },
  { id: '12', name: 'Hoa dã yên thảo', category: 'flower', price: 95000, image: 'https://images.unsplash.com/photo-1490750967868-88df5691cc9e?w=400', description: 'Hoa dã yên thảo nhiều màu sắc, thích hợp trồng ban công', difficulty: 'Trung bình', light: 'Nhiều ánh sáng', careLevel: 'Tưới vừa phải', rating: 4.4, reviews: 56, inStock: true, stock: 28 }
];

const MOCK_USERS = [
  { id: '1', name: 'Nguyễn Văn A', email: 'nguyenvana@email.com', phone: '0901234567', joinedDate: '2025-01-15', totalOrders: 12, totalSpent: 2500000 },
  { id: '2', name: 'Trần Thị B', email: 'tranthib@email.com', phone: '0912345678', joinedDate: '2025-02-20', totalOrders: 5, totalSpent: 850000 }
];

const MOCK_ORDERS = [
  { id: 'ORD001', userId: '1', items: [{ productId: '1', quantity: 2 }, { productId: '3', quantity: 1 }], total: 200000, status: 'completed', createdAt: '2026-02-28', shippingInfo: { fullName: 'Nguyễn Văn A', phone: '0901234567', address: '123 Đường ABC, Quận 1, TP.HCM' }, paymentMethod: 'COD' },
  { id: 'ORD002', userId: '2', items: [{ productId: '5', quantity: 1 }], total: 120000, status: 'shipping', createdAt: '2026-03-01', shippingInfo: { fullName: 'Trần Thị B', phone: '0912345678', address: '456 Đường XYZ, Quận 3, TP.HCM' }, paymentMethod: 'Chuyển khoản' },
  { id: 'ORD003', userId: '1', items: [{ productId: '2', quantity: 3 }, { productId: '6', quantity: 2 }], total: 195000, status: 'processing', createdAt: '2026-03-03', shippingInfo: { fullName: 'Nguyễn Văn A', phone: '0901234567', address: '123 Đường ABC, Quận 1, TP.HCM' }, paymentMethod: 'Ví điện tử' }
];

const BLOG_POSTS = [
  { id: '1', title: 'Cách trồng cà chua bi trên ban công', excerpt: 'Hướng dẫn chi tiết cách trồng cà chua bi tại nhà cho người mới bắt đầu...', image: 'https://images.unsplash.com/photo-1464226184884-fa280b87c399?w=600', author: 'Chuyên gia Vườn', date: '2026-02-15', category: 'Hướng dẫn' },
  { id: '2', title: '10 loại cây dễ trồng nhất cho người bận rộn', excerpt: 'Những loại cây không cần chăm sóc nhiều vẫn cho năng suất cao...', image: 'https://images.unsplash.com/photo-1416879595882-3373a0480b5b?w=600', author: 'Admin Vườn', date: '2026-02-20', category: 'Tips' },
  { id: '3', title: 'Chẩn đoán và xử lý bệnh vàng lá', excerpt: 'Nguyên nhân và cách khắc phục tình trạng lá cây bị vàng...', image: 'https://images.unsplash.com/photo-1558904541-efa843a96f01?w=600', author: 'Chuyên gia Vườn', date: '2026-02-25', category: 'Bệnh cây' }
];

const PROMOTIONS = [
  { id: '1', title: 'Giảm 20% cho đơn hàng đầu tiên', code: 'FIRSTORDER', discount: 20, description: 'Áp dụng cho khách hàng mới', validUntil: '2026-12-31', minOrder: 100000 },
  { id: '2', title: 'Miễn phí vận chuyển', code: 'FREESHIP', discount: 0, description: 'Cho đơn hàng từ 200k', validUntil: '2026-12-31', minOrder: 200000 }
];

const COMMUNITY_POSTS = [
  { id: '1', author: 'Minh Anh', avatar: 'https://images.unsplash.com/photo-1494790108377-be9c29b29330?w=100', content: 'Vườn cà chua bi của mình sau 2 tháng! Cảm ơn VƯƠN đã tư vấn nhiệt tình 🍅', image: 'https://images.unsplash.com/photo-1464226184884-fa280b87c399?w=500', likes: 45, comments: 12 },
  { id: '2', author: 'Tuấn Kiệt', avatar: 'https://images.unsplash.com/photo-1599566150163-29194dcaad36?w=100', content: 'Combo rau gia vị mua ở VƯƠN phát triển rất tốt. Bữa nào cũng có rau tươi!', image: 'https://images.unsplash.com/photo-1466692476868-aef1dfb1e735?w=500', likes: 32, comments: 8 }
];

const AI_QUESTIONS = [
  'Tôi nên trồng cây gì trên ban công?',
  'Làm sao để chăm sóc cây cà chua?',
  'Cây của tôi bị vàng lá phải làm sao?',
  'Loại đất nào phù hợp với cây hoa?',
  'Tưới nước bao nhiêu lần một ngày?'
];

const DEMO_CHAT_RESPONSES = {
  'trồng cà chua': `Hướng dẫn trồng cà chua bi tại nhà\n\n1. Chuẩn bị:\n- Chậu sâu tối thiểu 30cm\n- Đất trộn phân hữu cơ tỉ lệ 2:1\n\n2. Gieo hạt:\n- Gieo sâu 1–2cm, khoảng cách 15cm\n- Nảy mầm sau 7–10 ngày\n\n3. Chăm sóc:\n- Tưới 2 lần/ngày: sáng 6-7h & chiều 17-18h\n- Bón phân NPK mỗi 2 tuần\n\n4. Thu hoạch sau 60–80 ngày`,
  'tưới nước': `Hướng dẫn tưới nước đúng cách\n\n⏰ Thời điểm tốt nhất:\n- Sáng sớm: 6:00–7:00 (tốt nhất)\n- Chiều mát: 17:00–18:00\n- Tránh tưới: 10:00–15:00\n\n💡 Dấu hiệu tưới sai:\n- Lá vàng, rễ thối → tưới quá nhiều\n- Lá héo, mép cháy → tưới quá ít`,
  'vàng lá': `Nguyên nhân lá vàng và cách xử lý\n\n🔍 Nguyên nhân thường gặp:\n1. Thiếu Nitơ → bón phân NPK\n2. Tưới quá nhiều → giảm tần suất tưới\n3. Thiếu ánh sáng → di chuyển gần cửa sổ\n4. Đất cằn → thay đất mới\n\n✅ Giải pháp:\n- Loại bỏ lá vàng ngay\n- Kiểm tra độ ẩm đất trước khi tưới`,
  'ban công': `Gợi ý cây trồng trên ban công\n\n☀️ Ban công nhiều nắng:\n- Cà chua, ớt, húng quế\n- Hoa hướng dương, hoa dã yên thảo\n\n🌤 Ban công ít nắng:\n- Rau cải, xà lách\n- Cây dương xỉ, lan ý\n\n💡 Mẹo: Dùng chậu nhỏ gọn, xếp tầng để tiết kiệm diện tích!`,
  'default': 'Cảm ơn câu hỏi của bạn! Tôi là AI tư vấn trồng cây VƯƠN. Hãy hỏi tôi về cách trồng, chăm sóc cây, hay chẩn đoán bệnh cây nhé. Tôi sẵn sàng giúp bạn! 🌱'
};

// ── State Management (localStorage) ──────────────
const Store = {
  // User
  getUser() { try { return JSON.parse(localStorage.getItem('vuon_user') || 'null'); } catch { return null; } },
  setUser(u) { localStorage.setItem('vuon_user', JSON.stringify(u)); },
  clearUser() { localStorage.removeItem('vuon_user'); },

  // Cart
  getCart() { try { return JSON.parse(localStorage.getItem('vuon_cart') || '[]'); } catch { return []; } },
  setCart(c) { localStorage.setItem('vuon_cart', JSON.stringify(c)); },
  addToCart(product, qty = 1) {
    const cart = Store.getCart();
    const idx = cart.findIndex(i => String(i.productId) === String(product.id));
    if (idx >= 0) cart[idx].quantity += qty;
    else cart.push({ productId: product.id, product, quantity: qty });
    Store.setCart(cart);
    Store.updateCartBadge();
  },
  removeFromCart(productId) {
    Store.setCart(Store.getCart().filter(i => String(i.productId) !== String(productId)));
    Store.updateCartBadge();
  },
  updateCartQty(productId, qty) {
    if (qty <= 0) { Store.removeFromCart(productId); return; }
    const cart = Store.getCart();
    const idx = cart.findIndex(i => String(i.productId) === String(productId));
    if (idx >= 0) { cart[idx].quantity = qty; Store.setCart(cart); }
    Store.updateCartBadge();
  },
  clearCart() { Store.setCart([]); Store.updateCartBadge(); },
  getCartCount() { return Store.getCart().reduce((s, i) => s + i.quantity, 0); },
  getCartTotal() { return Store.getCart().reduce((s, i) => s + i.product.price * i.quantity, 0); },
  updateCartBadge() {
    const n = Store.getCartCount();
    document.querySelectorAll('.cart-count').forEach(el => { el.textContent = n; el.style.display = n > 0 ? 'flex' : 'none'; });
  },

  // Favorites
  getFavorites() { try { return JSON.parse(localStorage.getItem('vuon_favs') || '[]'); } catch { return []; } },
  toggleFavorite(id) {
    const favs = Store.getFavorites().map(String);
    const idx = favs.indexOf(String(id));
    if (idx >= 0) favs.splice(idx, 1); else favs.push(String(id));
    localStorage.setItem('vuon_favs', JSON.stringify(favs));
    return idx < 0; // returns true if added
  },
  isFavorite(id) { return Store.getFavorites().map(String).includes(String(id)); }
};

// ── Helpers ────────────────────────────────────
function fmt(num) { return num.toLocaleString('vi-VN') + 'đ'; }

function stars(rating) {
  let h = '';
  for (let i = 1; i <= 5; i++) h += `<svg width="16" height="16" fill="${i <= Math.floor(rating) ? '#facc15' : '#d1d5db'}" viewBox="0 0 24 24"><path d="M12 2l3.09 6.26L22 9.27l-5 4.87 1.18 6.88L12 17.77l-6.18 3.25L7 14.14 2 9.27l6.91-1.01L12 2z"/></svg>`;
  return h;
}

function getParam(name) { return new URLSearchParams(window.location.search).get(name); }

function getProduct(id) { return PRODUCTS.find(p => p.id === id); }

function statusLabel(s) {
  const map = { completed: 'Hoàn thành', shipping: 'Đang giao', processing: 'Đang xử lý', cancelled: 'Đã hủy' };
  const cls = { completed: 'badge-green', shipping: 'badge-blue', processing: 'badge-yellow', cancelled: 'badge-red' };
  return `<span class="badge ${cls[s] || 'badge-gray'}">${map[s] || s}</span>`;
}

// ── SVG Icons (inline) ─────────────────────────
const I = {
  home: '<svg width="20" height="20" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path d="M3 9l9-7 9 7v11a2 2 0 01-2 2H5a2 2 0 01-2-2z"/><polyline points="9 22 9 12 15 12 15 22"/></svg>',
  cart: '<svg width="20" height="20" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><circle cx="9" cy="21" r="1"/><circle cx="20" cy="21" r="1"/><path d="M1 1h4l2.68 13.39a2 2 0 001.94 1.61h9.72a2 2 0 001.94-1.61L23 6H6"/></svg>',
  heart: '<svg width="20" height="20" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path d="M20.84 4.61a5.5 5.5 0 00-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 00-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 000-7.78z"/></svg>',
  bot: '<svg width="20" height="20" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><rect x="3" y="11" width="18" height="11" rx="2"/><path d="M12 2v3M8 2h8M9 11V7h6v4"/><circle cx="9" cy="15" r="1"/><circle cx="15" cy="15" r="1"/></svg>',
  book: '<svg width="20" height="20" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path d="M4 19.5A2.5 2.5 0 016.5 17H20"/><path d="M6.5 2H20v20H6.5A2.5 2.5 0 014 19.5v-15A2.5 2.5 0 016.5 2z"/></svg>',
  users: '<svg width="20" height="20" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path d="M17 21v-2a4 4 0 00-4-4H5a4 4 0 00-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M23 21v-2a4 4 0 00-3-3.87M16 3.13a4 4 0 010 7.75"/></svg>',
  user: '<svg width="20" height="20" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path d="M20 21v-2a4 4 0 00-4-4H8a4 4 0 00-4 4v2"/><circle cx="12" cy="7" r="4"/></svg>',
  logout: '<svg width="20" height="20" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path d="M9 21H5a2 2 0 01-2-2V5a2 2 0 012-2h4M16 17l5-5-5-5M21 12H9"/></svg>',
  search: '<svg width="20" height="20" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><circle cx="11" cy="11" r="8"/><line x1="21" y1="21" x2="16.65" y2="16.65"/></svg>',
  leaf: '<svg width="32" height="32" fill="none" stroke="#fff" stroke-width="2" viewBox="0 0 24 24"><path d="M2 2s6 0 10 4c2 2 3 5 3 8M12 14c-2 4-5 6-8 6"/><path d="M22 2c-4 0-10 2-10 12"/></svg>',
  flower: '<svg width="32" height="32" fill="none" stroke="#fff" stroke-width="2" viewBox="0 0 24 24"><circle cx="12" cy="12" r="3"/><path d="M12 2C8 2 5 5 5 9c0 2.5 1.5 5 3 6.5L12 22l4-6.5c1.5-1.5 3-4 3-6.5 0-4-3-7-7-7z"/></svg>',
  box: '<svg width="32" height="32" fill="none" stroke="#fff" stroke-width="2" viewBox="0 0 24 24"><polyline points="21 8 21 21 3 21 3 8"/><rect x="1" y="3" width="22" height="5"/><line x1="10" y1="12" x2="14" y2="12"/></svg>',
  wrench: '<svg width="32" height="32" fill="none" stroke="#fff" stroke-width="2" viewBox="0 0 24 24"><path d="M14.7 6.3a1 1 0 000 1.4l1.6 1.6a1 1 0 001.4 0l3.77-3.77a6 6 0 01-7.94 7.94l-6.91 6.91a2.12 2.12 0 01-3-3l6.91-6.91a6 6 0 017.94-7.94l-3.76 3.76z"/></svg>',
  fb: '<svg width="16" height="16" fill="currentColor" viewBox="0 0 24 24"><path d="M18 2h-3a5 5 0 00-5 5v3H7v4h3v8h4v-8h3l1-4h-4V7a1 1 0 011-1h3z"/></svg>',
  ig: '<svg width="16" height="16" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><rect x="2" y="2" width="20" height="20" rx="5"/><path d="M16 11.37A4 4 0 1112.63 8 4 4 0 0116 11.37z"/><line x1="17.5" y1="6.5" x2="17.51" y2="6.5"/></svg>',
  yt: '<svg width="16" height="16" fill="currentColor" viewBox="0 0 24 24"><path d="M22.54 6.42a2.78 2.78 0 00-1.95-1.96C18.88 4 12 4 12 4s-6.88 0-8.59.46a2.78 2.78 0 00-1.95 1.96A29 29 0 001 12a29 29 0 00.46 5.58A2.78 2.78 0 003.41 19.6C5.12 20 12 20 12 20s6.88 0 8.59-.46a2.78 2.78 0 001.95-1.95A29 29 0 0023 12a29 29 0 00-.46-5.58z"/><polygon points="9.75 15.02 15.5 12 9.75 8.98 9.75 15.02" fill="white"/></svg>',
  phone: '<svg width="16" height="16" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path d="M22 16.92v3a2 2 0 01-2.18 2 19.79 19.79 0 01-8.63-3.07A19.5 19.5 0 013.07 9.81a19.79 19.79 0 01-3.07-8.67A2 2 0 012 0h3a2 2 0 012 1.72c.127.96.361 1.903.7 2.81a2 2 0 01-.45 2.11L6.09 7.91a16 16 0 006 6l1.27-1.27a2 2 0 012.11-.45c.907.339 1.85.573 2.81.7A2 2 0 0122 14.92z"/></svg>',
  mail: '<svg width="16" height="16" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path d="M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z"/><polyline points="22,6 12,13 2,6"/></svg>',
  map: '<svg width="16" height="16" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0118 0z"/><circle cx="12" cy="10" r="3"/></svg>',
  check: '<svg width="24" height="24" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><polyline points="20 6 9 17 4 12"/></svg>',
  trash: '<svg width="20" height="20" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><polyline points="3 6 5 6 21 6"/><path d="M19 6l-1 14a2 2 0 01-2 2H8a2 2 0 01-2-2L5 6M10 11v6M14 11v6M9 6V4a1 1 0 011-1h4a1 1 0 011 1v2"/></svg>',
  plus: '<svg width="16" height="16" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><line x1="12" y1="5" x2="12" y2="19"/><line x1="5" y1="12" x2="19" y2="12"/></svg>',
  minus: '<svg width="16" height="16" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><line x1="5" y1="12" x2="19" y2="12"/></svg>',
  arrowLeft: '<svg width="20" height="20" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><line x1="19" y1="12" x2="5" y2="12"/><polyline points="12 19 5 12 12 5"/></svg>',
  arrowRight: '<svg width="20" height="20" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><line x1="5" y1="12" x2="19" y2="12"/><polyline points="12 5 19 12 12 19"/></svg>',
  shield: '<svg width="32" height="32" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"/></svg>',
  sprout: '<svg width="48" height="48" fill="none" stroke="#fff" stroke-width="2" viewBox="0 0 24 24"><path d="M7 20h10M12 20V10m0 0C12 5 7 3 3 3s3 7 9 7zm0 0c0-5 5-7 9-7s-3 7-9 7"/></svg>',
  star: '<svg width="20" height="20" fill="#facc15" viewBox="0 0 24 24"><path d="M12 2l3.09 6.26L22 9.27l-5 4.87 1.18 6.88L12 17.77l-6.18 3.25L7 14.14 2 9.27l6.91-1.01L12 2z"/></svg>',
  send: '<svg width="20" height="20" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><line x1="22" y1="2" x2="11" y2="13"/><polygon points="22 2 15 22 11 13 2 9 22 2"/></svg>',
  camera: '<svg width="24" height="24" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path d="M23 19a2 2 0 01-2 2H3a2 2 0 01-2-2V8a2 2 0 012-2h4l2-3h6l2 3h4a2 2 0 012 2z"/><circle cx="12" cy="13" r="4"/></svg>',
  bell: '<svg width="24" height="24" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path d="M18 8A6 6 0 006 8c0 7-3 9-3 9h18s-3-2-3-9M13.73 21a2 2 0 01-3.46 0"/></svg>',
};

// ── Header Renderer ─────────────────────────────
function renderHeader() {
  const user = Store.getUser();
  const cartCount = Store.getCartCount();
  const base = getBasePath();

  document.getElementById('site-header').innerHTML = `
    <div class="header-inner">
      <a href="${base}home" class="logo">
        <div class="logo-icon">V</div>
        VƯƠN
      </a>
      <nav class="header-nav">
        <a href="${base}home">Trang chủ</a>
        <a href="${base}products?category=vegetable">Cây rau</a>
        <a href="${base}products?category=flower">Cây hoa</a>
        <a href="${base}ai-chat">AI Tư vấn</a>
        <a href="${base}blog">Blog</a>
      </nav>
      <div class="header-right">
        <div class="header-search-wrap" id="header-search-wrap">
          <div class="header-search-box">
            <span class="header-search-icon">${I.search}</span>
            <input
              type="text"
              id="header-search-input"
              class="header-search-input"
              placeholder="Tìm kiếm sản phẩm..."
              autocomplete="off"
            >
            <button id="header-search-clear" class="header-search-clear" style="display:none" title="Xóa">
              <svg width="16" height="16" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><line x1="18" y1="6" x2="6" y2="18"/><line x1="6" y1="6" x2="18" y2="18"/></svg>
            </button>
          </div>
          <div class="header-search-dropdown" id="header-search-dropdown"></div>
        </div>
        <a href="${base}cart" class="cart-btn" title="Giỏ hàng">
          ${I.cart}
          <span class="cart-badge cart-count" style="display:${cartCount > 0 ? 'flex' : 'none'}">${cartCount}</span>
        </a>
        ${user ? `
          <a href="${base}profile" style="font-size:.875rem;color:#374151;">${user.name}</a>
          <button onclick="doLogout()" style="padding:.5rem;border:none;background:none;cursor:pointer;display:flex;align-items:center;" title="Đăng xuất">${I.logout}</button>
        ` : `
          <a href="${base}login" class="btn btn-green" style="padding:.5rem 1rem;font-size:.875rem;">Đăng nhập</a>
        `}
      </div>
    </div>`;

  // Init live search after header injected
  _initHeaderSearch(base);
}

// ── Header Live Search Logic ─────────────────────
function _initHeaderSearch(base) {
  const input = document.getElementById('header-search-input');
  const dropdown = document.getElementById('header-search-dropdown');
  const clearBtn = document.getElementById('header-search-clear');
  if (!input || !dropdown) return;

  let debounceTimer = null;
  let lastQuery = '';

  // Sync with URL param if on products page
  const currentSearch = getParam('search');
  if (currentSearch) {
    input.value = currentSearch;
    clearBtn.style.display = 'flex';
  }

  input.addEventListener('input', () => {
    const q = input.value.trim();
    clearBtn.style.display = q ? 'flex' : 'none';
    clearTimeout(debounceTimer);
    if (!q) { _closeSearchDropdown(); lastQuery = ''; return; }
    debounceTimer = setTimeout(() => _doHeaderSearch(q, base), 300);
  });

  input.addEventListener('keydown', (e) => {
    if (e.key === 'Enter') {
      const q = input.value.trim();
      if (q) { _closeSearchDropdown(); _navigateToSearch(q, base); }
    }
    if (e.key === 'Escape') { _closeSearchDropdown(); input.blur(); }
  });

  clearBtn.addEventListener('click', () => {
    input.value = '';
    clearBtn.style.display = 'none';
    _closeSearchDropdown();
    // If on products page, reset filter
    document.dispatchEvent(new CustomEvent('header-search-change', { detail: { query: '' } }));
  });

  // Close dropdown when clicking outside
  document.addEventListener('click', (e) => {
    if (!document.getElementById('header-search-wrap')?.contains(e.target)) {
      _closeSearchDropdown();
    }
  });
}

async function _doHeaderSearch(q, base) {
  const dropdown = document.getElementById('header-search-dropdown');
  if (!dropdown) return;

  // Check if on products page — fire event instead of fetching dropdown
  const isProductsPage = window.location.pathname.includes('products') && !window.location.pathname.includes('admin');
  if (isProductsPage) {
    document.dispatchEvent(new CustomEvent('header-search-change', { detail: { query: q } }));
    _closeSearchDropdown();
    return;
  }

  dropdown.innerHTML = '<div class="hsd-loading"><span class="hsd-spinner"></span> Đang tìm...</div>';
  _openSearchDropdown();

  try {
    const results = await API.getProducts({ search: q, pageSize: 5 });
    if (!results || !results.length) {
      dropdown.innerHTML = `<div class="hsd-empty">Không tìm thấy "<b>${q}</b>"</div>`;
      return;
    }
    dropdown.innerHTML = results.map(p => `
      <a class="hsd-item" href="${base}product-detail?id=${p.id}">
        <img src="${p.image || 'https://placehold.co/40x40/f0fdf4/16a34a?text=🌱'}" alt="${p.name}" class="hsd-item-img" onerror="this.src='https://placehold.co/40x40/f0fdf4/16a34a?text=🌱'">
        <div class="hsd-item-info">
          <div class="hsd-item-name">${p.name}</div>
          <div class="hsd-item-price">${fmt(p.price)}</div>
        </div>
      </a>`).join('') +
      `<a class="hsd-view-all" href="${base}products?search=${encodeURIComponent(q)}">Xem tất cả kết quả →</a>`;
  } catch (e) {
    dropdown.innerHTML = '<div class="hsd-empty">Lỗi tìm kiếm. Thử lại nhé!</div>';
  }
}

function _openSearchDropdown() {
  const dd = document.getElementById('header-search-dropdown');
  if (dd) dd.classList.add('open');
}

function _closeSearchDropdown() {
  const dd = document.getElementById('header-search-dropdown');
  if (dd) dd.classList.remove('open');
}

function _navigateToSearch(q, base) {
  const isAdmin = window.location.pathname.startsWith('/admin');
  if (isAdmin) {
    window.location.href = '/admin/products?search=' + encodeURIComponent(q);
  } else {
    window.location.href = base + 'products?search=' + encodeURIComponent(q);
  }
}

// ── Mobile Nav Renderer ─────────────────────────
function renderMobileNav() {
  const base = getBasePath();
  const el = document.getElementById('mobile-nav');
  if (!el) return;
  el.innerHTML = `
    <a href="${base}home" class="mobile-nav-item">${I.home}<span>Trang chủ</span></a>
    <a href="${base}cart" class="mobile-nav-item" style="position:relative">
      ${I.cart}
      <span class="cart-count" style="position:absolute;top:2px;right:2px;background:#ef4444;color:#fff;font-size:.6rem;border-radius:50%;width:16px;height:16px;display:${Store.getCartCount() > 0 ? 'flex' : 'none'};align-items:center;justify-content:center">${Store.getCartCount()}</span>
      <span>Giỏ hàng</span>
    </a>
    <a href="${base}favorites" class="mobile-nav-item">${I.heart}<span>Yêu thích</span></a>
    <a href="${base}ai-chat" class="mobile-nav-item">${I.bot}<span>AI Tư vấn</span></a>
    <a href="${base}profile" class="mobile-nav-item">${I.user}<span>Tài khoản</span></a>`;
}

// ── Footer Renderer ─────────────────────────────
function renderFooter() {
  const base = getBasePath();
  const el = document.getElementById('site-footer');
  if (!el) return;
  el.innerHTML = `
    <div class="footer-newsletter">
      <div class="footer-newsletter-inner">
        <div>
          <h3>Nhận mẹo trồng cây mỗi tuần 🌱</h3>
          <p>Đăng ký để nhận hướng dẫn chăm sóc cây và ưu đãi độc quyền.</p>
        </div>
        <div class="footer-subscribe">
          <input type="email" placeholder="Nhập email của bạn...">
          <button>Đăng ký</button>
        </div>
      </div>
    </div>
    <div class="footer-links">
      <div class="footer-links-inner">
        <div class="footer-brand">
          <a href="${base}home.html" class="logo"><div class="logo-icon">V</div>VƯƠN</a>
          <p>Ứng dụng tư vấn và mua sắm cây trồng thông minh — kết hợp AI Gemini để giúp bạn chăm sóc vườn cây đúng cách mỗi ngày.</p>
          <div class="footer-contact">
            <a href="tel:18006789">${I.phone} 1800 6789 (Miễn phí)</a>
            <a href="mailto:Vuon@gmail.com">${I.mail} Vuon@gmail.com</a>
            <div>${I.map} Đại Học FPT - Hòa Lạc - Hà Nội</div>
          </div>
          <div class="footer-social">
            <a href="https://www.facebook.com/profile.php?id=61580801050695" target="_blank">${I.fb}</a>
            <a href="#">${I.ig}</a>
            <a href="#">${I.yt}</a>
          </div>
        </div>
        <div class="footer-col">
          <h4>Cửa hàng</h4>
          <ul>
            <li><a href="${base}products?category=vegetable">Cây rau</a></li>
            <li><a href="${base}products?category=flower">Cây hoa</a></li>
            <li><a href="${base}products?category=combo">Combo</a></li>
            <li><a href="${base}products?category=accessory">Phụ kiện</a></li>
            <li><a href="${base}promotions">Khuyến mãi</a></li>
          </ul>
        </div>
        <div class="footer-col">
          <h4>AI Tư Vấn</h4>
          <ul>
            <li><a href="${base}ai-chat">Chat với AI</a></li>
            <li><a href="${base}ai-recommend">Đề xuất cây phù hợp</a></li>
            <li><a href="${base}disease-diagnosis">Chẩn đoán bệnh cây</a></li>
            <li><a href="${base}care-guide">Hướng dẫn chăm sóc</a></li>
            <li><a href="${base}watering-reminder">Nhắc lịch tưới cây</a></li>
          </ul>
        </div>
        <div class="footer-col">
          <h4>Khám phá</h4>
          <ul>
            <li><a href="${base}blog">Blog trồng cây</a></li>
            <li><a href="${base}community">Cộng đồng</a></li>
            <li><a href="${base}favorites">Yêu thích</a></li>
            <li><a href="${base}search">Tìm kiếm</a></li>
          </ul>
        </div>
        <div class="footer-col">
          <h4>Hỗ trợ</h4>
          <ul>
            <li><a href="${base}support">Trung tâm hỗ trợ</a></li>
            <li><a href="${base}track-order">Theo dõi đơn hàng</a></li>
            <li><a href="${base}orders">Lịch sử đơn hàng</a></li>
            <li><a href="${base}profile">Tài khoản của tôi</a></li>
            <li><a href="${base}privacy">Chính sách bảo mật</a></li>
            <li><a href="${base}terms">Điều khoản sử dụng</a></li>
          </ul>
        </div>
      </div>
    </div>
    <div class="footer-bottom">
      <div class="footer-bottom-inner">
        <p>© ${new Date().getFullYear()} VƯƠN. Bảo lưu mọi quyền.</p>
        <div class="footer-payments">
          <span>Thanh toán an toàn:</span>
          ${['VISA', 'MC', 'MOMO', 'VNPAY', 'COD'].map(p => `<span class="payment-tag">${p}</span>`).join('')}
        </div>
      </div>
    </div>`;
}

// ── Base Path Detection ─────────────────────────
function getBasePath() {
  const path = window.location.pathname;
  if (path.startsWith('/admin') || path.includes('/admin/')) {
    return '/';
  }
  return './';
}

// ── Product Card HTML ───────────────────────────
function productCardHTML(p, base = '') {
  return `
    <div class="product-card">
      <a href="${base}product-detail?id=${p.id}">
        <div class="product-card-img"><img src="${p.image}" alt="${p.name}" loading="lazy"></div>
      </a>
      <div class="product-card-body">
        <a href="${base}product-detail?id=${p.id}" class="product-card-name">${p.name}</a>
        <div class="stars">${stars(p.rating)}<span style="font-size:.8rem;color:#6b7280;margin-left:.25rem">${p.rating} (${p.reviews})</span></div>
        <div class="product-card-footer">
          <span class="price">${fmt(p.price)}</span>
          <button class="add-to-cart-btn" onclick="addToCartToast('${p.id}')" title="Thêm vào giỏ">${I.cart}</button>
        </div>
      </div>
    </div>`;
}

// ── Add to Cart Toast ───────────────────────────
function addToCartToast(productId) {
  const p = getProduct(productId);
  if (!p) return;
  Store.addToCart(p);
  showToast('Đã thêm vào giỏ hàng! 🛒');
}

function showToast(msg, type = 'success') {
  let t = document.getElementById('toast');
  if (!t) { t = document.createElement('div'); t.id = 'toast'; t.style.cssText = 'position:fixed;bottom:80px;right:1rem;padding:.75rem 1.25rem;border-radius:.5rem;font-size:.875rem;z-index:9999;transition:opacity .3s;max-width:280px;box-shadow:0 4px 12px rgba(0,0,0,.15);'; document.body.appendChild(t); }
  t.textContent = msg;
  t.style.background = type === 'error' ? '#fee2e2' : type === 'warning' ? '#fefce8' : '#f0fdf4';
  t.style.color = type === 'error' ? '#991b1b' : type === 'warning' ? '#92400e' : '#166534';
  t.style.border = `1px solid ${type === 'error' ? '#fecaca' : type === 'warning' ? '#fde68a' : '#bbf7d0'}`;
  t.style.opacity = '1';
  clearTimeout(t._timer);
  t._timer = setTimeout(() => { t.style.opacity = '0'; }, 2500);
}

// ── Admin Sidebar ────────────────────────────────
function renderAdminSidebar() {
  const el = document.getElementById('admin-sidebar');
  if (!el) return;
  const path = window.location.pathname;
  const page = path.split('/').pop() || 'index.html';
  function active(p) { return page === p ? 'style="background:#16a34a;color:#fff;"' : 'style="color:#d1fae5;"'; }
  el.innerHTML = `
    <div id="admin-sidebar-inner" style="width:220px;background:#111827;min-height:100vh;padding:1.5rem 0;flex-shrink:0;">
      <div style="padding:0 1.25rem 1.5rem;border-bottom:1px solid #1f2937;">
        <div style="display:flex;align-items:center;gap:.5rem;">
          <div style="width:32px;height:32px;background:#16a34a;border-radius:.5rem;display:flex;align-items:center;justify-content:center;font-weight:800;color:#fff;">V</div>
          <span style="font-weight:700;color:#fff;">VƯƠN Admin</span>
        </div>
      </div>
      <nav style="padding:1rem 0;">
        <p style="font-size:.65rem;font-weight:600;color:#6b7280;text-transform:uppercase;padding:.25rem 1.25rem;margin-bottom:.25rem;">Tổng quan</p>
        ${[
      ['index.html', '📊', 'Dashboard'],
    ].map(([p, icon, label]) => `<a href="${p}" style="display:flex;align-items:center;gap:.75rem;padding:.6rem 1.25rem;text-decoration:none;border-radius:.375rem;margin:0 .5rem .25rem;font-size:.875rem;" ${active(p)}>${icon} ${label}</a>`).join('')}
        <p style="font-size:.65rem;font-weight:600;color:#6b7280;text-transform:uppercase;padding:.25rem 1.25rem;margin:.75rem 0 .25rem;">Quản lý</p>
        ${[
      ['products.html', '📦', 'Sản phẩm'],
      ['categories.html', '📁', 'Danh mục'],
      ['inventory.html', '🏭', 'Kho hàng'],
      ['orders.html', '🛒', 'Đơn hàng'],
      ['customers.html', '👥', 'Khách hàng'],
    ].map(([p, icon, label]) => `<a href="${p}" style="display:flex;align-items:center;gap:.75rem;padding:.6rem 1.25rem;text-decoration:none;border-radius:.375rem;margin:0 .5rem .25rem;font-size:.875rem;" ${active(p)}>${icon} ${label}</a>`).join('')}
        <p style="font-size:.65rem;font-weight:600;color:#6b7280;text-transform:uppercase;padding:.25rem 1.25rem;margin:.75rem 0 .25rem;">Nội dung</p>
        ${[
      ['ai.html', '🤖', 'AI & Dữ liệu'],
      ['faq.html', '❓', 'FAQ'],
      ['promotions.html', '🎁', 'Khuyến mãi'],
    ].map(([p, icon, label]) => `<a href="${p}" style="display:flex;align-items:center;gap:.75rem;padding:.6rem 1.25rem;text-decoration:none;border-radius:.375rem;margin:0 .5rem .25rem;font-size:.875rem;" ${active(p)}>${icon} ${label}</a>`).join('')}
      </nav>
      <div style="padding:1.25rem;border-top:1px solid #1f2937;margin-top:auto;">
        <a href="../home.html" style="display:flex;align-items:center;gap:.5rem;color:#9ca3af;text-decoration:none;font-size:.8rem;margin-bottom:.75rem;">← Về trang chủ</a>
        <button onclick="doLogout()" style="width:100%;padding:.5rem;background:#1f2937;color:#9ca3af;border:none;border-radius:.375rem;cursor:pointer;font-size:.8rem;">🚪 Đăng xuất</button>
      </div>
    </div>`;
}

// ── Auth ────────────────────────────────────────
function doLogout() {
  Store.clearUser();
  Store.clearCart();
  const base = getBasePath();
  window.location.href = base + 'login';
}

function requireAuth() {
  if (!Store.getUser()) {
    window.location.href = getBasePath() + 'login';
    return false;
  }
  return true;
}

// ── AI Chat Logic ───────────────────────────────
async function askGemini(userMsg, history = []) {
  // Try demo response first
  const low = userMsg.toLowerCase();
  for (const [k, v] of Object.entries(DEMO_CHAT_RESPONSES)) {
    if (k !== 'default' && low.includes(k)) return v;
  }
  try {
    const data = await apiFetch('/aichat/ask', {
      method: 'POST',
      body: JSON.stringify({
        userMsg: userMsg,
        history: history.map(h => ({ text: h.text, isBot: h.isBot }))
      })
    });
    if (data && data.reply) {
      return data.reply;
    }
    throw new Error('API error or empty reply');
  } catch (e) {
    console.error("AI Chat Error, falling back to mock:", e);
    return DEMO_CHAT_RESPONSES.default;
  }
}

// ── Init on DOM ready ────────────────────────────
document.addEventListener('DOMContentLoaded', () => {
  if (document.getElementById('site-header')) renderHeader();
  if (document.getElementById('mobile-nav')) renderMobileNav();
  if (document.getElementById('site-footer')) renderFooter();
  initCookieConsent();
});

function initCookieConsent() {
  if (localStorage.getItem('vuon_cookie_consent')) return;
  const banner = document.createElement('div');
  banner.id = 'cookie-consent-banner';
  banner.style.cssText = 'position:fixed;bottom:0;left:0;right:0;background:#1f2937;color:#fff;padding:1rem;z-index:99999;display:flex;justify-content:space-between;align-items:center;flex-wrap:wrap;gap:1rem;font-size:0.875rem;border-top:2px solid #16a34a;box-shadow:0 -4px 10px rgba(0,0,0,0.1);';
  banner.innerHTML = `
    <div style="flex:1;min-width:250px;">
      Website này sử dụng cookie để cải thiện trải nghiệm của bạn và phân tích lượng truy cập. Xem thêm tại 
      <a href="./privacy" style="color:#4ade80;text-decoration:underline;">Chính sách bảo mật</a>.
    </div>
    <div style="display:flex;gap:0.5rem;">
      <button id="cookie-consent-accept" style="background:#16a34a;color:#fff;border:none;padding:0.5rem 1rem;border-radius:0.375rem;cursor:pointer;font-weight:600;font-size:0.875rem;transition:background 0.2s;">Đồng ý</button>
    </div>
  `;
  document.body.appendChild(banner);
  document.getElementById('cookie-consent-accept').addEventListener('click', () => {
    localStorage.setItem('vuon_cookie_consent', 'accepted');
    banner.remove();
  });
}

// ─────────────────────────────────────────────────────────────────────────────
// BACKEND API LAYER
// Thử gọi API thật tại dynamic meta base URL hoặc fallback về localhost
// ─────────────────────────────────────────────────────────────────────────────
const apiMeta = document.querySelector('meta[name="api-base"]');
const API_BASE = apiMeta ? apiMeta.getAttribute('content') : 'http://localhost:5172/api';

// Lấy JWT token từ localStorage
function getToken() { return localStorage.getItem('vuon_token'); }
function saveToken(t) { localStorage.setItem('vuon_token', t); }
function clearToken() { localStorage.removeItem('vuon_token'); }

// Headers mặc định (có Bearer nếu đã đăng nhập)
function apiHeaders() {
  const h = { 'Content-Type': 'application/json' };
  const t = getToken();
  if (t) h['Authorization'] = 'Bearer ' + t;
  return h;
}

// Helper: fetch với timeout 15 giây
async function apiFetch(path, opts = {}) {
  const ctrl = new AbortController();
  const tid = setTimeout(() => ctrl.abort(), 15000);
  try {
    const res = await fetch(API_BASE + path, { ...opts, signal: ctrl.signal, headers: { ...apiHeaders(), ...(opts.headers || {}) } });
    clearTimeout(tid);

    if (res.status === 204) return null;
    if (res.status === 401) {
      clearToken();
      Store.clearUser();
      if (path.includes('/auth/login')) {
        let errJson = null;
        try {
          const contentType = res.headers.get("content-type");
          if (contentType && contentType.includes("application/json")) {
            errJson = await res.json();
          }
        } catch { }
        throw new Error((errJson && (errJson.Message || errJson.message)) || "Sai tài khoản hoặc mật khẩu");
      }
      throw new Error("Phiên đăng nhập đã hết hạn. Vui lòng đăng nhập lại.");
    }

    let json = null;
    const contentType = res.headers.get("content-type");
    if (contentType && contentType.includes("application/json")) {
      json = await res.json();
    }

    if (!res.ok) {
      throw new Error((json && (json.Message || json.message)) || `Lỗi hệ thống (${res.status})`);
    }
    return json;
  } finally {
    clearTimeout(tid);
  }
}

// ── API: Auth ────────────────────────────────────
const API = {

  async login(email, password) {
    try {
      const data = await apiFetch('/auth/login', {
        method: 'POST',
        body: JSON.stringify({ email, password })
      });
      // data là LoginResponseDto trực tiếp: { jwtToken, role, userId, fullName, email }
      saveToken(data.jwtToken);
      Store.setUser({ id: data.userId, name: data.fullName, email: data.email, role: data.role });
      return { ok: true, user: data };
    } catch (e) {
      return { ok: false, error: e.message };
    }
  },

  async register(name, email, password, phone) {
    try {
      const data = await apiFetch('/auth/register', {
        method: 'POST',
        body: JSON.stringify({ fullName: name, email, password, phoneNumber: phone })
      });
      return { ok: true, message: data?.message || data?.Message };
    } catch (e) {
      return { ok: false, error: e.message };
    }
  },

  async verifyOtp(email, otp) {
    try {
      await apiFetch('/auth/verify-otp', {
        method: 'POST',
        body: JSON.stringify({ email, otp })
      });
      return { ok: true };
    } catch (e) {
      return { ok: false, error: e.message };
    }
  },

  async forgotPassword(email) {
    try {
      await apiFetch('/auth/forgot-password', {
        method: 'POST',
        body: JSON.stringify({ email })
      });
      return { ok: true };
    } catch (e) {
      return { ok: false, error: e.message };
    }
  },

  async resetPassword(email, otp, newPassword) {
    try {
      await apiFetch('/auth/reset-password', {
        method: 'POST',
        body: JSON.stringify({ email, otp, newPassword })
      });
      return { ok: true };
    } catch (e) {
      return { ok: false, error: e.message };
    }
  },

  async getUsers() {
    try {
      return await apiFetch('/users');
    } catch (e) {
      console.error("Error fetching users:", e);
      throw e;
    }
  },

  async getDashboardStats() {
    try {
      return await apiFetch('/admin/dashboard/stats');
    } catch (e) {
      console.error("Error fetching dashboard stats:", e);
      throw e;
    }
  },

  // ── API: Categories ─────────────────────────────
  async getCategories() {
    try {
      return await apiFetch('/categories');
    } catch (e) {
      return [
        { categoryId: 1, name: 'Cây rau' },
        { categoryId: 2, name: 'Cây hoa' },
        { categoryId: 3, name: 'Combo' },
        { categoryId: 4, name: 'Phụ kiện' }
      ];
    }
  },

  async getCategory(id) {
    try {
      return await apiFetch('/categories/' + id);
    } catch (e) {
      const list = [
        { categoryId: 1, name: 'Cây rau' },
        { categoryId: 2, name: 'Cây hoa' },
        { categoryId: 3, name: 'Combo' },
        { categoryId: 4, name: 'Phụ kiện' }
      ];
      return list.find(c => c.categoryId == id) || null;
    }
  },

  async createCategory(name) {
    try {
      return await apiFetch('/categories', {
        method: 'POST',
        body: JSON.stringify({ name })
      });
    } catch (e) {
      return { ok: false, error: e.message };
    }
  },

  async updateCategory(id, name) {
    try {
      return await apiFetch('/categories/' + id, {
        method: 'PUT',
        body: JSON.stringify({ name })
      });
    } catch (e) {
      return { ok: false, error: e.message };
    }
  },

  async deleteCategory(id) {
    try {
      await apiFetch('/categories/' + id, {
        method: 'DELETE'
      });
      return { ok: true };
    } catch (e) {
      return { ok: false, error: e.message };
    }
  },

  // ── API: Products ──────────────────────────────
  // Map a ProductDto from backend to a normalised frontend object
  _mapProduct(p) {
    return {
      id: p.productId,
      productId: p.productId,
      name: p.name,
      categoryId: p.categoryId,
      category: p.categoryName || '',
      categoryName: p.categoryName || '',
      price: p.price,
      originalPrice: p.originalPrice || null,
      stock: p.stock,
      inStock: p.inStock,
      sku: p.sku || '',
      shortDescription: p.shortDescription || '',
      description: p.description || '',
      difficulty: p.difficulty || 'Dễ',
      careLevel: p.careLevel || '',
      rating: p.star || 5,
      reviews: 0,
      image: p.image || '',
      status: p.status
    };
  },

  async getProducts({ categoryId, search, page = 1, pageSize = 12, sortBy, isDescending = false } = {}) {
    try {
      const params = new URLSearchParams({ page, pageSize });
      if (categoryId) params.set('categoryId', categoryId);
      if (search) params.set('search', search);
      if (sortBy) params.set('sortBy', sortBy);
      params.set('isDescending', isDescending);
      const data = await apiFetch('/products?' + params);
      // data = { products: [...], totalCount, page, pageSize, totalPages }
      return (data.products || []).map(p => API._mapProduct(p));
    } catch (e) {
      // fallback mock
      let list = [...PRODUCTS];
      if (search) list = list.filter(p => p.name.toLowerCase().includes(search.toLowerCase()));
      return list;
    }
  },

  async getProduct(id) {
    try {
      const p = await apiFetch('/products/' + id);
      // data is a single ProductDto
      return { product: API._mapProduct(p), reviews: [] };
    } catch (e) {
      const p = PRODUCTS.find(x => x.id == id) || PRODUCTS[0];
      return { product: p, reviews: [] };
    }
  },

  async createProduct(productObj) {
    try {
      const data = await apiFetch('/products', {
        method: 'POST',
        body: JSON.stringify(productObj)
      });
      return { ok: true, data };
    } catch (e) {
      return { ok: false, error: e.message };
    }
  },

  async updateProduct(id, productObj) {
    try {
      const data = await apiFetch('/products/' + id, {
        method: 'PUT',
        body: JSON.stringify(productObj)
      });
      return { ok: true, data };
    } catch (e) {
      return { ok: false, error: e.message };
    }
  },

  async deleteProduct(id) {
    try {
      await apiFetch('/products/' + id, { method: 'DELETE' });
      return { ok: true };
    } catch (e) {
      return { ok: false, error: e.message };
    }
  },

  async getPromotions() {
    try {
      const data = await apiFetch('/promotions');
      return (Array.isArray(data) ? data : data.content || []).map(p => ({
        id: p.id, code: p.code, title: p.title,
        discount: p.discount, discountType: p.type,
        description: p.description, minOrder: p.minOrder,
        validUntil: p.validUntil ? p.validUntil.substring(0, 10) : '', isActive: p.active
      }));
    } catch (e) {
      return PROMOTIONS;
    }
  },

  // ── API: Orders ────────────────────────────────
  async createOrder(orderObj) {
    try {
      return await apiFetch('/orders', {
        method: 'POST',
        body: JSON.stringify(orderObj)
      });
    } catch (e) {
      throw e;
    }
  },

  async getMyOrders() {
    try {
      return await apiFetch('/orders/my');
    } catch (e) {
      return [];
    }
  },

  async getOrder(id) {
    try {
      return await apiFetch('/orders/' + id);
    } catch (e) {
      throw e;
    }
  },

  async cancelOrder(id) {
    try {
      return await apiFetch('/orders/' + id + '/cancel', {
        method: 'POST'
      });
    } catch (e) {
      throw e;
    }
  },

  async getOrders({ status, page = 1, pageSize = 15 } = {}) {
    try {
      const params = new URLSearchParams({ page, pageSize });
      if (status) params.set('status', status);
      return await apiFetch('/orders?' + params);
    } catch (e) {
      return { orders: [], totalCount: 0, page, pageSize, totalPages: 0 };
    }
  },

  async updateOrderStatus(id, status) {
    try {
      return await apiFetch('/orders/' + id + '/status', {
        method: 'PUT',
        body: JSON.stringify({ status })
      });
    } catch (e) {
      throw e;
    }
  },

  // ── Logout: xoá cả token ──────────────────────
  logout() {
    clearToken();
    Store.clearUser();
    Store.clearCart();
    window.location.href = getBasePath() + 'login';
  }
};

// Override doLogout để dùng API.logout
function doLogout() { API.logout(); }
