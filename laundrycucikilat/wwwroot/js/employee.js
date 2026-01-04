// Employee Dashboard JavaScript
let orders = [];
let filteredOrders = [];
let currentOrderId = '';
let currentOrderForPdf = null;

// Initialize page
document.addEventListener('DOMContentLoaded', function() {
    updateCurrentTime();
    setInterval(updateCurrentTime, 1000);
    loadOrders();
    
    // Setup event listeners
    document.getElementById('statusFilter').addEventListener('change', filterOrders);
    document.getElementById('searchInput').addEventListener('input', filterOrders);
});

// Update current time
function updateCurrentTime() {
    const now = new Date();
    const timeString = now.toLocaleTimeString('id-ID', {
        hour: '2-digit',
        minute: '2-digit',
        second: '2-digit'
    });
    document.getElementById('currentTime').textContent = timeString;
    document.getElementById('lastUpdated').textContent = now.toLocaleString('id-ID');
}

// Load orders from API
async function loadOrders() {
    try {
        showLoading();
        const response = await fetch('/api/employee/orders');
        if (response.ok) {
            orders = await response.json();
            filteredOrders = [...orders];
            displayOrders(filteredOrders);
            updateStats();
            updateOrderCount();
        } else {
            console.error('Failed to load orders:', response.status);
            showError('Gagal memuat data pesanan');
        }
    } catch (error) {
        console.error('Error loading orders:', error);
        showError('Terjadi kesalahan saat memuat data');
    }
}

// Display orders in table
function displayOrders(ordersToShow) {
    const tbody = document.getElementById('ordersTableBody');
    
    if (ordersToShow.length === 0) {
        tbody.innerHTML = `
            <tr>
                <td colspan="8" class="text-center py-5">
                    <div class="empty-state">
                        <i class="fas fa-inbox fa-3x text-muted mb-3"></i>
                        <h6 class="text-muted">Tidak ada pesanan ditemukan</h6>
                        <p class="text-muted mb-0">Coba ubah filter atau refresh data</p>
                    </div>
                </td>
            </tr>
        `;
        return;
    }

    tbody.innerHTML = ordersToShow.map(order => `
        <tr class="fade-in">
            <td class="px-3 py-3">
                <div class="d-flex flex-column">
                    <strong class="text-primary">${order.orderId}</strong>
                    <small class="text-muted">${formatDateTime(order.createdAt)}</small>
                </div>
            </td>
            <td class="px-3 py-3">
                <div class="d-flex flex-column">
                    <strong>${order.namaLengkap}</strong>
                    <small class="text-muted">
                        <i class="fas fa-phone fa-xs me-1"></i>${order.noTelepon}
                    </small>
                </div>
            </td>
            <td class="px-3 py-3">
                <div class="d-flex align-items-center">
                    <i class="fas fa-tshirt text-primary me-2"></i>
                    <span>${order.jenisLayanan}</span>
                </div>
            </td>
            <td class="px-3 py-3">
                <div class="d-flex align-items-center">
                    <i class="fas fa-weight text-info me-2"></i>
                    <strong>${order.jumlah} kg</strong>
                </div>
            </td>
            <td class="px-3 py-3">
                <strong class="text-success">${order.total}</strong>
            </td>
            <td class="px-3 py-3">
                <span class="badge ${getStatusBadgeClass(order.status)} px-3 py-2">
                    ${getStatusIcon(order.status)} ${order.status}
                </span>
            </td>
            <td class="px-3 py-3">
                <div class="d-flex flex-column">
                    <small class="text-muted">Pesan: ${formatDate(order.tanggalPesan)}</small>
                    <small class="text-muted">Ambil: ${order.tanggalAmbil}</small>
                </div>
            </td>
            <td class="px-3 py-3">
                <div class="btn-group btn-group-sm" role="group">
                    <button class="btn btn-outline-primary" onclick="viewOrderDetail('${order.orderId}')" 
                            title="Lihat Detail" data-bs-toggle="tooltip">
                        <i class="fas fa-eye"></i>
                    </button>
                    <button class="btn btn-outline-info" onclick="downloadOrderPdf('${order.orderId}')" 
                            title="Download PDF" data-bs-toggle="tooltip">
                        <i class="fas fa-file-pdf"></i>
                    </button>
                    <button class="btn btn-outline-success" onclick="showUpdateStatusModal('${order.orderId}', '${order.status}')" 
                            title="Update Status" data-bs-toggle="tooltip">
                        <i class="fas fa-edit"></i>
                    </button>
                    <button class="btn btn-outline-danger" onclick="showDeleteConfirmModal('${order.orderId}')" 
                            title="Hapus Pesanan" data-bs-toggle="tooltip">
                        <i class="fas fa-trash"></i>
                    </button>
                </div>
            </td>
        </tr>
    `).join('');

    // Initialize tooltips
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });
}

// Get status badge class
function getStatusBadgeClass(status) {
    const statusClasses = {
        'Menunggu Konfirmasi': 'bg-warning text-dark',
        'Diterima': 'bg-info',
        'Dicuci': 'bg-primary',
        'Disetrika': 'bg-secondary',
        'Selesai': 'bg-success'
    };
    return statusClasses[status] || 'bg-secondary';
}

// Get status icon
function getStatusIcon(status) {
    const statusIcons = {
        'Menunggu Konfirmasi': '⏳',
        'Diterima': '✅',
        'Dicuci': '🧼',
        'Disetrika': '👔',
        'Selesai': '✨'
    };
    return statusIcons[status] || '📋';
}

// Format date
function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString('id-ID', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric'
    });
}

// Format date time
function formatDateTime(dateString) {
    const date = new Date(dateString);
    return date.toLocaleString('id-ID', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    });
}

// Update statistics
function updateStats() {
    const today = new Date().toISOString().split('T')[0];
    
    const todayOrders = orders.filter(order => 
        order.tanggalPesan === today
    ).length;
    
    const pendingOrders = orders.filter(order => 
        order.status === 'Menunggu Konfirmasi'
    ).length;
    
    const processingOrders = orders.filter(order => 
        ['Diterima', 'Dicuci', 'Disetrika'].includes(order.status)
    ).length;
    
    const completedToday = orders.filter(order => 
        order.status === 'Selesai' && order.tanggalPesan === today
    ).length;

    document.getElementById('todayOrders').textContent = todayOrders;
    document.getElementById('pendingOrders').textContent = pendingOrders;
    document.getElementById('processingOrders').textContent = processingOrders;
    document.getElementById('completedOrders').textContent = completedToday;
}

// Update order count
function updateOrderCount() {
    document.getElementById('totalOrders').textContent = filteredOrders.length;
}

// Filter orders by status and search
function filterOrders() {
    const selectedStatus = document.getElementById('statusFilter').value;
    const searchTerm = document.getElementById('searchInput').value.toLowerCase();
    
    filteredOrders = orders.filter(order => {
        const matchesStatus = selectedStatus === '' || order.status === selectedStatus;
        const matchesSearch = searchTerm === '' || 
            order.orderId.toLowerCase().includes(searchTerm) ||
            order.namaLengkap.toLowerCase().includes(searchTerm) ||
            order.noTelepon.includes(searchTerm) ||
            order.jenisLayanan.toLowerCase().includes(searchTerm);
        
        return matchesStatus && matchesSearch;
    });
    
    displayOrders(filteredOrders);
    updateOrderCount();
}

// Refresh orders
function refreshOrders() {
    loadOrders();
    showSuccess('Data pesanan berhasil diperbarui');
}

// View order detail
async function viewOrderDetail(orderId) {
    try {
        console.log('Viewing order detail for:', orderId);
        showLoadingModal();
        
        const response = await fetch(`/api/employee/orders/${orderId}`);
        console.log('Response status:', response.status);
        
        if (response.ok) {
            const order = await response.json();
            console.log('Order data:', order);
            showOrderDetailModal(order);
        } else {
            const errorData = await response.text();
            console.error('Error response:', errorData);
            showError(`Gagal memuat detail pesanan: ${response.status}`);
        }
    } catch (error) {
        console.error('Error loading order detail:', error);
        showError('Terjadi kesalahan saat memuat detail pesanan');
    }
}

// Show loading in modal
function showLoadingModal() {
    const modalContent = document.getElementById('orderDetailContent');
    modalContent.innerHTML = `
        <div class="text-center py-5">
            <div class="spinner-border text-primary mb-3" role="status">
                <span class="visually-hidden">Loading...</span>
            </div>
            <p class="text-muted mb-0">Memuat detail pesanan...</p>
        </div>
    `;
    
    const modal = new bootstrap.Modal(document.getElementById('orderDetailModal'));
    modal.show();
}

// Show order detail modal
function showOrderDetailModal(order) {
    const modalContent = document.getElementById('orderDetailContent');
    
    // Store current order for PDF generation
    currentOrderForPdf = order;
    
    // Ensure all fields have default values
    const safeOrder = {
        orderId: order.orderId || 'N/A',
        namaLengkap: order.namaLengkap || 'N/A',
        noTelepon: order.noTelepon || 'N/A',
        alamat: order.alamat || 'N/A',
        jenisLayanan: order.jenisLayanan || 'N/A',
        jumlah: order.jumlah || 0,
        total: order.total || 'Rp 0',
        status: order.status || 'Unknown',
        tanggalPesan: order.tanggalPesan || 'N/A',
        estimasiSelesai: order.estimasiSelesai || 'N/A',
        tanggalAmbil: order.tanggalAmbil || 'N/A',
        waktuAmbil: order.waktuAmbil || 'N/A',
        antarJemput: order.antarJemput || false,
        pewangiKhusus: order.pewangiKhusus || false,
        catatan: order.catatan || '',
        paymentStatus: order.paymentStatus || 'Belum Lunas',
        paymentMethod: order.paymentMethod || 'N/A',
        createdAt: order.createdAt || order.tanggalPesan || 'N/A'
    };
    
    modalContent.innerHTML = `
        <div class="row">
            <div class="col-lg-6">
                <div class="card border-primary mb-3">
                    <div class="card-header bg-primary text-white">
                        <h6 class="mb-0"><i class="fas fa-info-circle me-2"></i>Informasi Pesanan</h6>
                    </div>
                    <div class="card-body">
                        <table class="table table-sm table-borderless">
                            <tr>
                                <td class="fw-bold" width="40%">ID Pesanan:</td>
                                <td><span class="badge bg-primary fs-6">${safeOrder.orderId}</span></td>
                            </tr>
                            <tr>
                                <td class="fw-bold">Tanggal Pesan:</td>
                                <td>${formatDate(safeOrder.tanggalPesan)}</td>
                            </tr>
                            <tr>
                                <td class="fw-bold">Status Saat Ini:</td>
                                <td><span class="badge ${getStatusBadgeClass(safeOrder.status)} fs-6">${getStatusIcon(safeOrder.status)} ${safeOrder.status}</span></td>
                            </tr>
                            <tr>
                                <td class="fw-bold">Estimasi Selesai:</td>
                                <td><strong class="text-success">${safeOrder.estimasiSelesai}</strong></td>
                            </tr>
                            <tr>
                                <td class="fw-bold">Tanggal Ambil:</td>
                                <td>${safeOrder.tanggalAmbil} <strong>${safeOrder.waktuAmbil}</strong></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <div class="col-lg-6">
                <div class="card border-success mb-3">
                    <div class="card-header bg-success text-white">
                        <h6 class="mb-0"><i class="fas fa-user me-2"></i>Informasi Pelanggan</h6>
                    </div>
                    <div class="card-body">
                        <table class="table table-sm table-borderless">
                            <tr>
                                <td class="fw-bold" width="40%">Nama Lengkap:</td>
                                <td><strong>${safeOrder.namaLengkap}</strong></td>
                            </tr>
                            <tr>
                                <td class="fw-bold">No. Telepon:</td>
                                <td>
                                    <a href="tel:${safeOrder.noTelepon}" class="text-decoration-none">
                                        <i class="fas fa-phone text-success me-1"></i>${safeOrder.noTelepon}
                                    </a>
                                </td>
                            </tr>
                            <tr>
                                <td class="fw-bold">Alamat:</td>
                                <td>${safeOrder.alamat}</td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-12">
                <div class="card border-info">
                    <div class="card-header bg-info text-white">
                        <h6 class="mb-0"><i class="fas fa-tshirt me-2"></i>Detail Layanan Laundry</h6>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-6">
                                <table class="table table-sm table-borderless">
                                    <tr>
                                        <td class="fw-bold" width="40%">Jenis Layanan:</td>
                                        <td><span class="badge bg-info fs-6">${safeOrder.jenisLayanan}</span></td>
                                    </tr>
                                    <tr>
                                        <td class="fw-bold">Berat Laundry:</td>
                                        <td><strong class="text-primary">${safeOrder.jumlah} kg</strong></td>
                                    </tr>
                                    <tr>
                                        <td class="fw-bold">Antar Jemput:</td>
                                        <td>
                                            ${safeOrder.antarJemput ? 
                                                '<span class="badge bg-success">✅ Ya</span>' : 
                                                '<span class="badge bg-secondary">❌ Tidak</span>'
                                            }
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="col-md-6">
                                <table class="table table-sm table-borderless">
                                    <tr>
                                        <td class="fw-bold" width="40%">Pewangi Khusus:</td>
                                        <td>
                                            ${safeOrder.pewangiKhusus ? 
                                                '<span class="badge bg-success">✅ Ya</span>' : 
                                                '<span class="badge bg-secondary">❌ Tidak</span>'
                                            }
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fw-bold">Total Biaya:</td>
                                        <td><h5 class="text-success mb-0">${safeOrder.total}</h5></td>
                                    </tr>
                                    <tr>
                                        <td class="fw-bold">Status Pembayaran:</td>
                                        <td>
                                            <span class="badge ${safeOrder.paymentStatus === 'Lunas' ? 'bg-success' : 'bg-warning'} fs-6">
                                                ${safeOrder.paymentStatus}
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fw-bold">Metode Pembayaran:</td>
                                        <td>
                                            <span class="badge bg-primary fs-6">
                                                ${safeOrder.paymentMethod}
                                            </span>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        ${safeOrder.catatan ? `
                            <div class="mt-3">
                                <div class="alert alert-light border">
                                    <h6 class="alert-heading"><i class="fas fa-sticky-note me-2"></i>Catatan Khusus:</h6>
                                    <p class="mb-0">${safeOrder.catatan}</p>
                                </div>
                            </div>
                        ` : ''}
                    </div>
                </div>
            </div>
        </div>
        
        <!-- Action Buttons -->
        <div class="row mt-3">
            <div class="col-12">
                <div class="d-flex gap-2 justify-content-end">
                    <button class="btn btn-info" onclick="downloadOrderPdf('${safeOrder.orderId}')">
                        <i class="fas fa-file-pdf me-1"></i>Download PDF
                    </button>
                    <button class="btn btn-success" onclick="showUpdateStatusModal('${safeOrder.orderId}', '${safeOrder.status}')">
                        <i class="fas fa-edit me-1"></i>Update Status
                    </button>
                    <button class="btn btn-danger" onclick="showDeleteConfirmModal('${safeOrder.orderId}')">
                        <i class="fas fa-trash me-1"></i>Hapus Pesanan
                    </button>
                </div>
            </div>
        </div>
    `;
    
    // Modal is already shown from showLoadingModal, just update content
}

// Show update status modal
function showUpdateStatusModal(orderId, currentStatus) {
    currentOrderId = orderId;
    const modal = new bootstrap.Modal(document.getElementById('updateStatusModal'));
    document.getElementById('updateOrderId').value = orderId;
    document.getElementById('currentStatus').value = currentStatus;
    document.getElementById('newStatus').value = '';
    modal.show();
}

// Update order status
async function updateOrderStatus() {
    const orderId = document.getElementById('updateOrderId').value;
    const newStatus = document.getElementById('newStatus').value;
    
    if (!newStatus) {
        showError('Pilih status baru terlebih dahulu');
        return;
    }
    
    try {
        const response = await fetch(`/api/employee/orders/${orderId}/status`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ status: newStatus })
        });
        
        if (response.ok) {
            showSuccess(`Status pesanan berhasil diubah menjadi "${newStatus}"`);
            bootstrap.Modal.getInstance(document.getElementById('updateStatusModal')).hide();
            loadOrders(); // Reload orders
        } else {
            showError('Gagal memperbarui status pesanan');
        }
    } catch (error) {
        console.error('Error updating order status:', error);
        showError('Terjadi kesalahan saat memperbarui status');
    }
}

// Show delete confirmation modal
function showDeleteConfirmModal(orderId) {
    console.log('Showing delete confirmation for:', orderId);
    const order = orders.find(o => o.orderId === orderId);
    if (!order) {
        showError('Pesanan tidak ditemukan');
        return;
    }
    
    document.getElementById('deleteOrderId').value = orderId;
    document.getElementById('deleteOrderDetails').innerHTML = `
        <div class="row mb-2">
            <div class="col-6"><strong>ID Pesanan:</strong></div>
            <div class="col-6">${order.orderId}</div>
        </div>
        <div class="row mb-2">
            <div class="col-6"><strong>Pelanggan:</strong></div>
            <div class="col-6">${order.namaLengkap}</div>
        </div>
        <div class="row mb-2">
            <div class="col-6"><strong>Layanan:</strong></div>
            <div class="col-6">${order.jenisLayanan}</div>
        </div>
        <div class="row mb-2">
            <div class="col-6"><strong>Berat:</strong></div>
            <div class="col-6">${order.jumlah} kg</div>
        </div>
        <div class="row mb-2">
            <div class="col-6"><strong>Total:</strong></div>
            <div class="col-6"><strong class="text-success">${order.total}</strong></div>
        </div>
        <div class="row">
            <div class="col-6"><strong>Status:</strong></div>
            <div class="col-6"><span class="badge ${getStatusBadgeClass(order.status)}">${order.status}</span></div>
        </div>
    `;
    
    const modal = new bootstrap.Modal(document.getElementById('deleteConfirmModal'));
    modal.show();
}

// Confirm delete order
async function confirmDeleteOrder() {
    const orderId = document.getElementById('deleteOrderId').value;
    console.log('Deleting order:', orderId);
    
    try {
        const response = await fetch(`/api/employee/orders/${orderId}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json'
            }
        });
        
        console.log('Delete response status:', response.status);
        
        if (response.ok) {
            showSuccess('Pesanan berhasil dihapus');
            bootstrap.Modal.getInstance(document.getElementById('deleteConfirmModal')).hide();
            
            // Close detail modal if open
            const detailModal = bootstrap.Modal.getInstance(document.getElementById('orderDetailModal'));
            if (detailModal) {
                detailModal.hide();
            }
            
            loadOrders(); // Reload orders
        } else {
            const errorData = await response.text();
            console.error('Delete error:', errorData);
            showError(`Gagal menghapus pesanan: ${response.status}`);
        }
    } catch (error) {
        console.error('Error deleting order:', error);
        showError('Terjadi kesalahan saat menghapus pesanan');
    }
}

// Print order detail
function printOrderDetail() {
    window.print();
}

// Download order PDF
async function downloadOrderPdf(orderId) {
    try {
        console.log('Downloading PDF for order:', orderId);
        
        // Show loading state
        let downloadBtn = null;
        let originalText = '';
        
        // Find the button that was clicked
        if (event && event.target) {
            downloadBtn = event.target.closest('button');
            if (downloadBtn) {
                originalText = downloadBtn.innerHTML;
                downloadBtn.innerHTML = '<i class="fas fa-spinner fa-spin me-1"></i>Generating...';
                downloadBtn.disabled = true;
            }
        }
        
        const response = await fetch(`/api/employee/orders/${orderId}/pdf`);
        
        if (response.ok) {
            // Get the PDF blob
            const blob = await response.blob();
            
            // Create download link
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            
            // Get filename from response headers or create default
            const contentDisposition = response.headers.get('Content-Disposition');
            let filename = `Detail_Pesanan_${orderId}_${new Date().toISOString().slice(0,10)}.pdf`;
            
            if (contentDisposition) {
                const filenameMatch = contentDisposition.match(/filename="(.+)"/);
                if (filenameMatch) {
                    filename = filenameMatch[1];
                }
            }
            
            link.download = filename;
            document.body.appendChild(link);
            link.click();
            
            // Cleanup
            document.body.removeChild(link);
            window.URL.revokeObjectURL(url);
            
            showSuccess('PDF berhasil didownload');
        } else {
            const errorData = await response.text();
            console.error('PDF generation error:', errorData);
            showError(`Gagal membuat PDF: ${response.status}`);
        }
    } catch (error) {
        console.error('Error downloading PDF:', error);
        showError('Terjadi kesalahan saat membuat PDF');
    } finally {
        // Restore button state
        if (downloadBtn && originalText) {
            downloadBtn.innerHTML = originalText;
            downloadBtn.disabled = false;
        }
    }
}

// Download PDF from modal
function downloadPdfFromModal() {
    if (currentOrderForPdf) {
        downloadOrderPdf(currentOrderForPdf.orderId);
    }
}

// Alternative function to open PDF in new tab
function viewOrderPdf(orderId) {
    const pdfUrl = `/api/employee/orders/${orderId}/pdf`;
    window.open(pdfUrl, '_blank');
}

// Show loading
function showLoading() {
    const tbody = document.getElementById('ordersTableBody');
    tbody.innerHTML = `
        <tr>
            <td colspan="8" class="text-center py-5">
                <div class="spinner-border text-primary mb-3" role="status">
                    <span class="visually-hidden">Loading...</span>
                </div>
                <p class="text-muted mb-0">Memuat data pesanan...</p>
            </td>
        </tr>
    `;
}

// Show success message with toast
function showSuccess(message) {
    // Create toast element
    const toastHtml = `
        <div class="toast align-items-center text-white bg-success border-0" role="alert" aria-live="assertive" aria-atomic="true">
            <div class="d-flex">
                <div class="toast-body">
                    <i class="fas fa-check-circle me-2"></i>${message}
                </div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
            </div>
        </div>
    `;
    
    // Add to toast container or create one
    let toastContainer = document.getElementById('toastContainer');
    if (!toastContainer) {
        toastContainer = document.createElement('div');
        toastContainer.id = 'toastContainer';
        toastContainer.className = 'toast-container position-fixed top-0 end-0 p-3';
        toastContainer.style.zIndex = '9999';
        document.body.appendChild(toastContainer);
    }
    
    toastContainer.insertAdjacentHTML('beforeend', toastHtml);
    const toastElement = toastContainer.lastElementChild;
    const toast = new bootstrap.Toast(toastElement);
    toast.show();
    
    // Remove toast element after it's hidden
    toastElement.addEventListener('hidden.bs.toast', () => {
        toastElement.remove();
    });
}

// Show error message with toast
function showError(message) {
    // Create toast element
    const toastHtml = `
        <div class="toast align-items-center text-white bg-danger border-0" role="alert" aria-live="assertive" aria-atomic="true">
            <div class="d-flex">
                <div class="toast-body">
                    <i class="fas fa-exclamation-circle me-2"></i>${message}
                </div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
            </div>
        </div>
    `;
    
    // Add to toast container or create one
    let toastContainer = document.getElementById('toastContainer');
    if (!toastContainer) {
        toastContainer = document.createElement('div');
        toastContainer.id = 'toastContainer';
        toastContainer.className = 'toast-container position-fixed top-0 end-0 p-3';
        toastContainer.style.zIndex = '9999';
        document.body.appendChild(toastContainer);
    }
    
    toastContainer.insertAdjacentHTML('beforeend', toastHtml);
    const toastElement = toastContainer.lastElementChild;
    const toast = new bootstrap.Toast(toastElement);
    toast.show();
    
    // Remove toast element after it's hidden
    toastElement.addEventListener('hidden.bs.toast', () => {
        toastElement.remove();
    });
}