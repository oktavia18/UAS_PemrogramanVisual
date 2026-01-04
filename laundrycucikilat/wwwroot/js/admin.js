// Admin Dashboard JavaScript
let dashboardData = {};
let services = [];
let customers = [];
let employees = [];
let reportData = {};

// Initialize page
document.addEventListener('DOMContentLoaded', function() {
    loadDashboardData();
    
    // Setup tab change events
    document.querySelectorAll('#adminTabs button[data-bs-toggle="tab"]').forEach(tab => {
        tab.addEventListener('shown.bs.tab', function(event) {
            const target = event.target.getAttribute('data-bs-target');
            switch(target) {
                case '#services':
                    loadServices();
                    break;
                case '#customers':
                    loadCustomers();
                    break;
                case '#employees':
                    loadEmployees();
                    break;
                case '#reports':
                    generateReport();
                    break;
            }
        });
    });

    // Setup delete confirmation checkbox
    const confirmDeleteCheckbox = document.getElementById('confirmDeleteEmployee');
    if (confirmDeleteCheckbox) {
        confirmDeleteCheckbox.addEventListener('change', function() {
            const confirmBtn = document.getElementById('confirmDeleteBtn');
            confirmBtn.disabled = !this.checked;
        });
    }

    // Setup phone number validation for edit form
    const editPhoneInput = document.getElementById('editEmployeePhone');
    if (editPhoneInput) {
        editPhoneInput.addEventListener('input', function() {
            // Remove any non-digit, non-space, non-hyphen, non-plus, non-parentheses characters
            this.value = this.value.replace(/[^\d\s\-\+\(\)]/g, '');
        });
    }

    // Setup phone number validation for add form
    const addPhoneInput = document.getElementById('employeePhone');
    if (addPhoneInput) {
        addPhoneInput.addEventListener('input', function() {
            // Remove any non-digit, non-space, non-hyphen, non-plus, non-parentheses characters
            this.value = this.value.replace(/[^\d\s\-\+\(\)]/g, '');
        });
    }
});

// Load dashboard data
async function loadDashboardData() {
    try {
        const response = await fetch('/api/admin/dashboard');
        if (response.ok) {
            dashboardData = await response.json();
            updateDashboard();
        } else {
            showError('Gagal memuat data dashboard');
        }
    } catch (error) {
        console.error('Error loading dashboard:', error);
        showError('Terjadi kesalahan saat memuat dashboard');
    }
}

// Update dashboard display
function updateDashboard() {
    document.getElementById('dashTotalOrders').textContent = dashboardData.totalOrders || 0;
    document.getElementById('dashTotalCustomers').textContent = dashboardData.totalCustomers || 0;
    document.getElementById('dashTotalEmployees').textContent = dashboardData.totalEmployees || 0;
    document.getElementById('dashTotalRevenue').textContent = formatCurrency(dashboardData.totalRevenue || 0);
    document.getElementById('dashTodayOrders').textContent = dashboardData.todayOrders || 0;
    document.getElementById('dashPendingOrders').textContent = dashboardData.pendingOrders || 0;
    document.getElementById('dashCompletedOrders').textContent = dashboardData.completedOrders || 0;
    
    // Update recent orders table
    const recentOrdersTable = document.getElementById('recentOrdersTable');
    if (dashboardData.recentOrders && dashboardData.recentOrders.length > 0) {
        recentOrdersTable.innerHTML = dashboardData.recentOrders.map(order => `
            <tr>
                <td><small>${order.orderId}</small></td>
                <td><small>${order.namaLengkap}</small></td>
                <td><small>${order.jenisLayanan}</small></td>
                <td><span class="badge ${getStatusBadgeClass(order.status)} badge-sm">${order.status}</span></td>
                <td><small>${order.total}</small></td>
            </tr>
        `).join('');
    } else {
        recentOrdersTable.innerHTML = '<tr><td colspan="5" class="text-center">Tidak ada pesanan terbaru</td></tr>';
    }
}

// Load services
async function loadServices() {
    try {
        const response = await fetch('/api/admin/services');
        if (response.ok) {
            services = await response.json();
            displayServices();
        } else {
            showError('Gagal memuat data layanan');
        }
    } catch (error) {
        console.error('Error loading services:', error);
        showError('Terjadi kesalahan saat memuat data layanan');
    }
}

// Display services
function displayServices() {
    const tbody = document.getElementById('servicesTable');
    
    if (services.length === 0) {
        tbody.innerHTML = '<tr><td colspan="7" class="text-center">Belum ada layanan</td></tr>';
        return;
    }
    
    tbody.innerHTML = services.map(service => `
        <tr>
            <td><strong>${service.serviceId}</strong></td>
            <td>${service.namaLayanan}</td>
            <td>${service.kategori}</td>
            <td>${formatCurrency(service.hargaPerKg)}</td>
            <td>${service.estimasiWaktu}</td>
            <td><span class="badge ${service.status === 'Aktif' ? 'bg-success' : 'bg-secondary'}">${service.status}</span></td>
            <td>
                <div class="btn-group btn-group-sm">
                    <button class="btn btn-outline-primary" onclick="editService('${service.serviceId}')" title="Edit">
                        <i class="fas fa-edit"></i>
                    </button>
                    <button class="btn btn-outline-danger" onclick="deleteService('${service.serviceId}')" title="Hapus">
                        <i class="fas fa-trash"></i>
                    </button>
                </div>
            </td>
        </tr>
    `).join('');
}

// Add new service
async function addService() {
    const serviceData = {
        namaLayanan: document.getElementById('serviceName').value,
        deskripsi: document.getElementById('serviceDescription').value,
        kategori: document.getElementById('serviceCategory').value,
        hargaPerKg: parseFloat(document.getElementById('servicePrice').value),
        estimasiWaktu: document.getElementById('serviceEstimation').value
    };
    
    if (!serviceData.namaLayanan || !serviceData.kategori || !serviceData.hargaPerKg || !serviceData.estimasiWaktu) {
        showError('Harap lengkapi semua field yang wajib diisi');
        return;
    }
    
    try {
        const response = await fetch('/api/admin/services', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(serviceData)
        });
        
        if (response.ok) {
            showSuccess('Layanan berhasil ditambahkan');
            bootstrap.Modal.getInstance(document.getElementById('addServiceModal')).hide();
            document.getElementById('addServiceForm').reset();
            loadServices();
        } else {
            showError('Gagal menambahkan layanan');
        }
    } catch (error) {
        console.error('Error adding service:', error);
        showError('Terjadi kesalahan saat menambahkan layanan');
    }
}

// Load customers
async function loadCustomers() {
    try {
        const response = await fetch('/api/admin/customers');
        if (response.ok) {
            customers = await response.json();
            displayCustomers();
        } else {
            showError('Gagal memuat data pelanggan');
        }
    } catch (error) {
        console.error('Error loading customers:', error);
        showError('Terjadi kesalahan saat memuat data pelanggan');
    }
}

// Display customers
function displayCustomers() {
    const tbody = document.getElementById('customersTable');
    
    if (customers.length === 0) {
        tbody.innerHTML = '<tr><td colspan="7" class="text-center">Belum ada data pelanggan</td></tr>';
        return;
    }
    
    tbody.innerHTML = customers.map(customer => `
        <tr>
            <td><strong>${customer.customerId}</strong></td>
            <td>${customer.namaLengkap}</td>
            <td>${customer.noTelepon}</td>
            <td>${customer.email || '-'}</td>
            <td>${customer.totalPesanan}</td>
            <td>${formatCurrency(customer.totalBelanja)}</td>
            <td><span class="badge ${customer.status === 'Aktif' ? 'bg-success' : 'bg-secondary'}">${customer.status}</span></td>
        </tr>
    `).join('');
}

// Load employees
async function loadEmployees() {
    try {
        const response = await fetch('/api/admin/employees');
        if (response.ok) {
            employees = await response.json();
            displayEmployees();
        } else {
            showError('Gagal memuat data karyawan');
        }
    } catch (error) {
        console.error('Error loading employees:', error);
        showError('Terjadi kesalahan saat memuat data karyawan');
    }
}

// Display employees
function displayEmployees() {
    const tbody = document.getElementById('employeesTable');
    
    if (employees.length === 0) {
        tbody.innerHTML = '<tr><td colspan="8" class="text-center">Belum ada data karyawan</td></tr>';
        return;
    }
    
    tbody.innerHTML = employees.map(employee => `
        <tr>
            <td><strong>${employee.employeeId}</strong></td>
            <td>${employee.namaLengkap}</td>
            <td>${employee.jabatan}</td>
            <td>${employee.noTelepon}</td>
            <td>${employee.email}</td>
            <td>${formatDate(employee.tanggalMasuk)}</td>
            <td>
                <span class="badge ${employee.status === 'Aktif' ? 'bg-success' : 'bg-secondary'}">
                    ${employee.status}
                </span>
            </td>
            <td>
                <div class="btn-group btn-group-sm">
                    <button class="btn btn-outline-primary" onclick="editEmployee('${employee.employeeId}')" title="Edit">
                        <i class="fas fa-edit"></i>
                    </button>
                    <button class="btn btn-outline-danger" onclick="deleteEmployee('${employee.employeeId}')" title="Hapus Permanen">
                        <i class="fas fa-trash"></i>
                    </button>
                </div>
            </td>
        </tr>
    `).join('');
}

// Add new employee
async function addEmployee() {
    try {
        // Validate required fields
        const requiredFields = [
            { id: 'employeeName', name: 'Nama Lengkap' },
            { id: 'employeeEmail', name: 'Email' },
            { id: 'employeePhone', name: 'No. Telepon' },
            { id: 'employeePosition', name: 'Jabatan' },
            { id: 'employeeSalary', name: 'Gaji' }
        ];

        for (const field of requiredFields) {
            const value = document.getElementById(field.id).value.trim();
            if (!value) {
                showError(`${field.name} wajib diisi`);
                document.getElementById(field.id).focus();
                return;
            }
        }

        // Validate phone number
        const phoneNumber = document.getElementById('employeePhone').value.trim();
        if (!/^[\d\s\-\+\(\)]+$/.test(phoneNumber)) {
            showError('Nomor telepon hanya boleh berisi angka, spasi, tanda hubung, dan tanda kurung');
            document.getElementById('employeePhone').focus();
            return;
        }

        // Validate email format
        const email = document.getElementById('employeeEmail').value.trim();
        if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
            showError('Format email tidak valid');
            document.getElementById('employeeEmail').focus();
            return;
        }

        // Validate salary
        const salary = parseFloat(document.getElementById('employeeSalary').value);
        if (isNaN(salary) || salary < 0) {
            showError('Gaji harus berupa angka positif');
            document.getElementById('employeeSalary').focus();
            return;
        }

        const employeeData = {
            namaLengkap: document.getElementById('employeeName').value.trim(),
            email: email,
            noTelepon: phoneNumber,
            jabatan: document.getElementById('employeePosition').value,
            alamat: document.getElementById('employeeAddress').value.trim(),
            gaji: salary,
            tanggalMasuk: new Date().toISOString().split('T')[0]
        };
        
        console.log('Adding new employee:', employeeData);

        // Show loading state
        const addBtn = event.target;
        const originalText = addBtn.innerHTML;
        addBtn.innerHTML = '<i class="fas fa-spinner fa-spin me-1"></i>Menyimpan...';
        addBtn.disabled = true;
        
        const response = await fetch('/api/admin/employees', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(employeeData)
        });
        
        if (response.ok) {
            const result = await response.json();
            showSuccess(result.message || 'Karyawan berhasil ditambahkan');
            bootstrap.Modal.getInstance(document.getElementById('addEmployeeModal')).hide();
            document.getElementById('addEmployeeForm').reset();
            await loadEmployees();
        } else {
            const errorData = await response.json();
            showError(errorData.message || 'Gagal menambahkan karyawan');
        }
    } catch (error) {
        console.error('Error adding employee:', error);
        showError('Terjadi kesalahan saat menambahkan karyawan: ' + error.message);
    } finally {
        // Restore button state
        const addBtn = event.target;
        if (addBtn) {
            addBtn.innerHTML = originalText || 'Simpan';
            addBtn.disabled = false;
        }
    }
}

// Generate report
async function generateReport() {
    const startDate = document.getElementById('startDate').value;
    const endDate = document.getElementById('endDate').value;
    
    let url = '/api/admin/reports/transactions';
    if (startDate && endDate) {
        url += `?startDate=${startDate}&endDate=${endDate}`;
    }
    
    try {
        const response = await fetch(url);
        if (response.ok) {
            reportData = await response.json();
            displayReport();
        } else {
            showError('Gagal memuat laporan');
        }
    } catch (error) {
        console.error('Error generating report:', error);
        showError('Terjadi kesalahan saat memuat laporan');
    }
}

// Display report
function displayReport() {
    document.getElementById('reportTotalTransactions').textContent = reportData.totalTransactions || 0;
    document.getElementById('reportTotalRevenue').textContent = formatCurrency(reportData.totalRevenue || 0);
    document.getElementById('reportCompletedOrders').textContent = reportData.completedOrders || 0;
    document.getElementById('reportPendingOrders').textContent = reportData.pendingOrders || 0;
    
    const tbody = document.getElementById('reportsTable');
    
    if (reportData.transactions && reportData.transactions.length > 0) {
        tbody.innerHTML = reportData.transactions.map(order => `
            <tr>
                <td><strong>${order.orderId}</strong></td>
                <td>${order.namaLengkap}</td>
                <td>${order.jenisLayanan}</td>
                <td>${order.total}</td>
                <td><span class="badge ${getStatusBadgeClass(order.status)}">${order.status}</span></td>
                <td>${formatDate(order.tanggalPesan)}</td>
                <td>
                    <button class="btn btn-sm btn-outline-primary" onclick="printReceipt('${order.orderId}')" title="Cetak Struk">
                        <i class="fas fa-print"></i>
                    </button>
                </td>
            </tr>
        `).join('');
    } else {
        tbody.innerHTML = '<tr><td colspan="7" class="text-center">Tidak ada data transaksi</td></tr>';
    }
}

// Export to PDF - Updated with full implementation
async function exportToPDF() {
    console.log('exportToPDF function called - Version 2024.12.04'); // Debug log
    
    try {
        // Get date filter values
        const startDate = document.getElementById('startDate').value;
        const endDate = document.getElementById('endDate').value;

        console.log('Date values:', { startDate, endDate }); // Debug log

        // Validate dates
        if (!startDate || !endDate) {
            showError('Silakan pilih tanggal awal dan tanggal akhir terlebih dahulu');
            return;
        }

        if (new Date(startDate) > new Date(endDate)) {
            showError('Tanggal awal tidak boleh lebih besar dari tanggal akhir');
            return;
        }

        // Show loading state
        const exportBtn = event.target;
        const originalText = exportBtn.innerHTML;
        exportBtn.innerHTML = '<i class="fas fa-spinner fa-spin me-1"></i>Generating PDF...';
        exportBtn.disabled = true;

        console.log('Starting PDF export...'); // Debug log

        // Build URL with parameters
        const params = new URLSearchParams({
            startDate: startDate,
            endDate: endDate
        });

        const url = `/api/admin/reports/transactions/export-pdf?${params.toString()}`;
        console.log('API URL:', url); // Debug log

        // Fetch PDF
        const response = await fetch(url);
        console.log('API Response status:', response.status); // Debug log

        if (response.ok) {
            // Get the PDF blob
            const blob = await response.blob();
            console.log('PDF blob size:', blob.size); // Debug log
            
            // Create download link
            const downloadUrl = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = downloadUrl;
            
            // Get filename from response headers or create default
            const contentDisposition = response.headers.get('Content-Disposition');
            let filename = `Laporan_Transaksi_${startDate}_${endDate}_${new Date().toISOString().slice(0,10)}.pdf`;
            
            if (contentDisposition) {
                const filenameMatch = contentDisposition.match(/filename="(.+)"/);
                if (filenameMatch) {
                    filename = filenameMatch[1];
                }
            }
            
            console.log('Download filename:', filename); // Debug log
            
            link.download = filename;
            document.body.appendChild(link);
            link.click();
            
            // Cleanup
            document.body.removeChild(link);
            window.URL.revokeObjectURL(downloadUrl);
            
            showSuccess('Laporan PDF berhasil didownload!');
            console.log('PDF download completed successfully'); // Debug log
        } else {
            const errorData = await response.text();
            console.error('PDF export error:', errorData);
            showError(`Gagal membuat PDF: ${response.status} - ${errorData}`);
        }
    } catch (error) {
        console.error('Error exporting PDF:', error);
        showError('Terjadi kesalahan saat membuat PDF: ' + error.message);
    } finally {
        // Restore button state
        const exportBtn = event.target;
        if (exportBtn) {
            exportBtn.innerHTML = originalText || '<i class="fas fa-file-pdf me-1"></i>Export PDF';
            exportBtn.disabled = false;
        }
    }
}

// Print receipt
function printReceipt(orderId) {
    window.open(`/Struk?orderId=${orderId}`, '_blank');
}

// Utility functions
function getStatusBadgeClass(status) {
    const statusClasses = {
        'Menunggu Konfirmasi': 'bg-warning',
        'Diterima': 'bg-info',
        'Dicuci': 'bg-primary',
        'Disetrika': 'bg-secondary',
        'Selesai': 'bg-success'
    };
    return statusClasses[status] || 'bg-secondary';
}

function formatCurrency(amount) {
    return new Intl.NumberFormat('id-ID', {
        style: 'currency',
        currency: 'IDR',
        minimumFractionDigits: 0
    }).format(amount);
}

function formatDate(dateString) {
    const date = new Date(dateString);
    return date.toLocaleDateString('id-ID', {
        day: '2-digit',
        month: '2-digit',
        year: 'numeric'
    });
}

function showSuccess(message) {
    // You can implement a toast notification here
    alert(message);
}

function showError(message) {
    // You can implement a toast notification here
    alert(message);
}

// Download All Reports PDF - New function for downloading complete data
async function downloadAllReportsPDF() {
    console.log('downloadAllReportsPDF function called'); // Debug log
    
    try {
        // Show loading state
        const downloadBtn = event.target;
        const originalText = downloadBtn.innerHTML;
        downloadBtn.innerHTML = '<i class="fas fa-spinner fa-spin me-1"></i>Generating PDF...';
        downloadBtn.disabled = true;

        console.log('Starting complete PDF export...'); // Debug log

        // API endpoint for all data
        const url = '/api/admin/reports/all-transactions/export-pdf';
        console.log('API URL:', url); // Debug log

        // Fetch PDF
        const response = await fetch(url);
        console.log('API Response status:', response.status); // Debug log

        if (response.ok) {
            // Get the PDF blob
            const blob = await response.blob();
            console.log('PDF blob size:', blob.size); // Debug log
            
            // Create download link
            const downloadUrl = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = downloadUrl;
            
            // Get filename from response headers or create default
            const contentDisposition = response.headers.get('Content-Disposition');
            let filename = `laporan-transaksi-semua-data_${new Date().toISOString().slice(0,10)}.pdf`;
            
            if (contentDisposition) {
                const filenameMatch = contentDisposition.match(/filename="(.+)"/);
                if (filenameMatch) {
                    filename = filenameMatch[1];
                }
            }
            
            console.log('Download filename:', filename); // Debug log
            
            link.download = filename;
            document.body.appendChild(link);
            link.click();
            
            // Cleanup
            document.body.removeChild(link);
            window.URL.revokeObjectURL(downloadUrl);
            
            showSuccess('Laporan lengkap PDF berhasil didownload!');
            console.log('Complete PDF download completed successfully'); // Debug log
        } else {
            const errorData = await response.text();
            console.error('Complete PDF export error:', errorData);
            showError(`Gagal membuat PDF lengkap: ${response.status} - ${errorData}`);
        }
    } catch (error) {
        console.error('Error downloading complete PDF:', error);
        showError('Terjadi kesalahan saat membuat PDF lengkap: ' + error.message);
    } finally {
        // Restore button state
        const downloadBtn = event.target;
        if (downloadBtn) {
            downloadBtn.innerHTML = originalText || '<i class="fas fa-download me-1"></i>Download Semua Laporan (PDF)';
            downloadBtn.disabled = false;
        }
    }
}

// Placeholder functions for edit and delete
function editService(serviceId) {
    showSuccess('Fitur edit layanan akan segera tersedia');
}

function deleteService(serviceId) {
    if (confirm('Apakah Anda yakin ingin menghapus layanan ini?')) {
        // Implement delete service
        showSuccess('Layanan berhasil dihapus');
    }
}

// Edit Employee - Load employee data and show edit modal
async function editEmployee(employeeId) {
    try {
        console.log('=== EDIT EMPLOYEE FUNCTION CALLED ===');
        console.log('Employee ID:', employeeId);
        console.log('Event target:', event.target);
        
        // Show loading state
        const editBtn = event.target.closest('button');
        const originalContent = editBtn.innerHTML;
        editBtn.innerHTML = '<i class="fas fa-spinner fa-spin"></i>';
        editBtn.disabled = true;

        console.log('Fetching employee data from API...');
        
        // Fetch employee data
        const response = await fetch(`/api/admin/employees/${employeeId}`);
        console.log('API Response status:', response.status);
        
        if (response.ok) {
            const employee = await response.json();
            console.log('Employee data loaded:', employee);
            
            // Check if modal exists
            const editModal = document.getElementById('editEmployeeModal');
            if (!editModal) {
                console.error('Edit modal not found in DOM!');
                showError('Modal edit tidak ditemukan');
                return;
            }
            
            // Populate edit form
            document.getElementById('editEmployeeId').value = employee.employeeId;
            document.getElementById('editEmployeeName').value = employee.namaLengkap || '';
            document.getElementById('editEmployeeEmail').value = employee.email || '';
            document.getElementById('editEmployeePhone').value = employee.noTelepon || '';
            document.getElementById('editEmployeePosition').value = employee.jabatan || '';
            document.getElementById('editEmployeeAddress').value = employee.alamat || '';
            document.getElementById('editEmployeeSalary').value = employee.gaji || '';
            document.getElementById('editEmployeeJoinDate').value = employee.tanggalMasuk || '';
            document.getElementById('editEmployeeStatus').value = employee.status || 'Aktif';
            
            console.log('Form populated, showing modal...');
            
            // Show edit modal
            const modal = new bootstrap.Modal(editModal);
            modal.show();
            
            console.log('Edit modal should be visible now');
            
        } else {
            const errorData = await response.json();
            console.error('API Error:', errorData);
            showError(errorData.message || 'Gagal memuat data karyawan');
        }
    } catch (error) {
        console.error('Exception in editEmployee:', error);
        showError('Terjadi kesalahan saat memuat data karyawan: ' + error.message);
    } finally {
        // Restore button state
        const editBtn = event.target.closest('button');
        if (editBtn) {
            editBtn.innerHTML = originalContent;
            editBtn.disabled = false;
        }
    }
}

// Update Employee - Save changes
async function updateEmployee() {
    try {
        const employeeId = document.getElementById('editEmployeeId').value;
        
        // Validate required fields
        const requiredFields = [
            { id: 'editEmployeeName', name: 'Nama Lengkap' },
            { id: 'editEmployeeEmail', name: 'Email' },
            { id: 'editEmployeePhone', name: 'No. Telepon' },
            { id: 'editEmployeePosition', name: 'Jabatan' },
            { id: 'editEmployeeStatus', name: 'Status' }
        ];

        for (const field of requiredFields) {
            const value = document.getElementById(field.id).value.trim();
            if (!value) {
                showError(`${field.name} wajib diisi`);
                document.getElementById(field.id).focus();
                return;
            }
        }

        // Validate phone number
        const phoneNumber = document.getElementById('editEmployeePhone').value.trim();
        if (!/^[\d\s\-\+\(\)]+$/.test(phoneNumber)) {
            showError('Nomor telepon hanya boleh berisi angka, spasi, tanda hubung, dan tanda kurung');
            document.getElementById('editEmployeePhone').focus();
            return;
        }

        // Validate email format
        const email = document.getElementById('editEmployeeEmail').value.trim();
        if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
            showError('Format email tidak valid');
            document.getElementById('editEmployeeEmail').focus();
            return;
        }

        // Prepare employee data
        const employeeData = {
            employeeId: employeeId, // This will be ignored by backend (readonly)
            namaLengkap: document.getElementById('editEmployeeName').value.trim(),
            email: email,
            noTelepon: phoneNumber,
            jabatan: document.getElementById('editEmployeePosition').value,
            alamat: document.getElementById('editEmployeeAddress').value.trim(),
            gaji: parseFloat(document.getElementById('editEmployeeSalary').value) || 0,
            tanggalMasuk: document.getElementById('editEmployeeJoinDate').value,
            status: document.getElementById('editEmployeeStatus').value
        };

        console.log('Updating employee:', employeeData);

        // Show loading state
        const saveBtn = event.target;
        const originalText = saveBtn.innerHTML;
        saveBtn.innerHTML = '<i class="fas fa-spinner fa-spin me-1"></i>Menyimpan...';
        saveBtn.disabled = true;

        // Send update request
        const response = await fetch(`/api/admin/employees/${employeeId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(employeeData)
        });

        if (response.ok) {
            const result = await response.json();
            showSuccess(result.message || 'Data karyawan berhasil diperbarui');
            
            // Hide modal
            bootstrap.Modal.getInstance(document.getElementById('editEmployeeModal')).hide();
            
            // Reload employees data
            await loadEmployees();
            
        } else {
            const errorData = await response.json();
            showError(errorData.message || 'Gagal memperbarui data karyawan');
        }
    } catch (error) {
        console.error('Error updating employee:', error);
        showError('Terjadi kesalahan saat memperbarui data karyawan: ' + error.message);
    } finally {
        // Restore button state
        const saveBtn = event.target;
        if (saveBtn) {
            saveBtn.innerHTML = originalText;
            saveBtn.disabled = false;
        }
    }
}

// Delete Employee - Show confirmation modal
function deleteEmployee(employeeId) {
    try {
        console.log('=== DELETE EMPLOYEE FUNCTION CALLED ===');
        console.log('Employee ID:', employeeId);
        console.log('Available employees:', employees);
        
        // Find employee data
        const employee = employees.find(emp => emp.employeeId === employeeId);
        if (!employee) {
            console.error('Employee not found in employees array:', employeeId);
            showError('Data karyawan tidak ditemukan');
            return;
        }

        console.log('Employee found:', employee);

        // Check if modal exists
        const deleteModal = document.getElementById('deleteEmployeeModal');
        if (!deleteModal) {
            console.error('Delete modal not found in DOM!');
            showError('Modal konfirmasi tidak ditemukan');
            return;
        }

        // Populate delete confirmation modal
        document.getElementById('deleteEmployeeId').textContent = employee.employeeId;
        document.getElementById('deleteEmployeeName').textContent = employee.namaLengkap;
        document.getElementById('deleteEmployeePosition').textContent = employee.jabatan;
        document.getElementById('deleteEmployeeEmail').textContent = employee.email;
        
        // Reset confirmation checkbox
        const confirmCheckbox = document.getElementById('confirmDeleteEmployee');
        confirmCheckbox.checked = false;
        document.getElementById('confirmDeleteBtn').disabled = true;
        
        // Store employee ID for deletion
        document.getElementById('confirmDeleteBtn').setAttribute('data-employee-id', employeeId);
        
        console.log('Modal populated, showing delete confirmation...');
        
        // Show delete confirmation modal
        const modal = new bootstrap.Modal(deleteModal);
        modal.show();
        
        console.log('Delete confirmation modal should be visible now');
        
    } catch (error) {
        console.error('Exception in deleteEmployee:', error);
        showError('Terjadi kesalahan saat mempersiapkan penghapusan: ' + error.message);
    }
}

// Confirm Delete Employee - Perform hard delete
async function confirmDeleteEmployee() {
    try {
        const employeeId = document.getElementById('confirmDeleteBtn').getAttribute('data-employee-id');
        
        if (!employeeId) {
            showError('ID karyawan tidak ditemukan');
            return;
        }

        console.log('Confirming HARD DELETE for employee:', employeeId);

        // Show loading state
        const confirmBtn = document.getElementById('confirmDeleteBtn');
        const originalText = confirmBtn.innerHTML;
        confirmBtn.innerHTML = '<i class="fas fa-spinner fa-spin me-1"></i>Menghapus...';
        confirmBtn.disabled = true;

        // Send delete request (hard delete)
        const response = await fetch(`/api/admin/employees/${employeeId}`, {
            method: 'DELETE'
        });

        if (response.ok) {
            const result = await response.json();
            showSuccess(result.message || 'Karyawan berhasil dihapus permanen');
            
            // Hide modal
            bootstrap.Modal.getInstance(document.getElementById('deleteEmployeeModal')).hide();
            
            // Remove employee from local array (since it's deleted from database)
            employees = employees.filter(emp => emp.employeeId !== employeeId);
            
            // Update display immediately without reloading from server
            displayEmployees();
            
            // Also update dashboard if needed
            loadDashboardData();
            
        } else {
            const errorData = await response.json();
            showError(errorData.message || 'Gagal menghapus karyawan');
        }
    } catch (error) {
        console.error('Error deleting employee:', error);
        showError('Terjadi kesalahan saat menghapus karyawan: ' + error.message);
    } finally {
        // Restore button state
        const confirmBtn = document.getElementById('confirmDeleteBtn');
        if (confirmBtn) {
            confirmBtn.innerHTML = originalText;
            confirmBtn.disabled = false;
        }
    }
}