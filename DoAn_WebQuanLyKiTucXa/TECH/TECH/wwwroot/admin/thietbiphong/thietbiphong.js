(function ($) {
    var self = this;
    self.IsUpdate = false;

    // Load danh sách thiết bị theo phòng
    self.loadThietBiTheoPhong = function (phongId) {
        if (!phongId) {
            tedu.notify("Vui lòng chọn phòng để tìm kiếm", "warning");
            return;
        }
        $.ajax({
            url: '/Admin/ThietBiPhong/GetThietBiPhongByPhongId',
            type: 'GET',
            data: { phongId: phongId },
            dataType: 'json',
            success: function (res) {
                renderTable(res.data);
            },
            error: function () {
                tedu.notify("Lỗi khi lấy danh sách thiết bị", "error");
            }
        });
    };

    // Vẽ bảng
    function renderTable(data) {
        var html = "";
        if (data && data.length > 0) {
            $.each(data, function (i, item) {
                html += "<tr>";
                html += "<td>" + (i + 1) + "</td>";
                html += "<td>" + (item.TenThietBi || "") + "</td>";
                html += "<td>" + (item.LoaiThietBi || "") + "</td>";
                html += "<td>" + (item.NgayCap ? new Date(item.NgayCap).toLocaleDateString() : "") + "</td>";
                html += "<td>" + (item.GhiChu || "") + "</td>";
                html += "<td class='text-center'>";
                html += `<button class='btn btn-primary btn-repair' data-phongid='${item.MaPhong}' data-thietbiid='${item.MaThietBi}'>Sửa chữa</button> `;
                html += `<button class='btn btn-warning btn-edit' data-id='${item.MaThietBi}'>Sửa</button> `;
                html += `<button class='btn btn-danger btn-delete' data-id='${item.MaThietBi}'>Xóa</button>`;
                html += "</td></tr>";
            });
        } else {
            html = "<tr><td colspan='6' class='text-center'>Không có dữ liệu</td></tr>";
        }
        $("#tblData").html(html);
    }

    // Nút tìm kiếm
    $("#btnSearch").click(function () {
        var phongId = $('#phongSelect').val();
        self.loadThietBiTheoPhong(phongId);
    });

    // Mở modal thêm mới
    $(".btn-addorupdate").click(function () {
        var phongId = $('#phongSelect').val();
        if (!phongId) {
            tedu.notify("Vui lòng chọn phòng trước khi thêm", "warning");
            return;
        }

        // Reset form
        $('#form-submit')[0].reset();
        $('#MaPhong').val(phongId);
        $('#MaThietBi').val('');
        $('#NgayCap').val('');
        $('#GhiChu').val('');
        $('#titleModal').text('Thêm thiết bị phòng');
        $('.btn-submit-format').text('Thêm mới');
        self.IsUpdate = false;

        $('#userModal').modal('show');
    });

    // Xử lý sửa
    $(document).on('click', '.btn-edit', function () {
        var phongId = $('#phongSelect').val();
        var maThietBi = $(this).data('id');
        if (!phongId || !maThietBi) {
            tedu.notify("Thiếu thông tin để sửa", "warning");
            return;
        }

        $.ajax({
            url: '/Admin/ThietBiPhong/AddOrUpdate',
            type: 'GET',
            data: { phongId: phongId, thietBiId: maThietBi },
            success: function (res) {
                if (res.success && res.data) {
                    var data = res.data;
                    $('#MaPhong').val(data.MaPhong);
                    $('#MaThietBi').val(data.MaThietBi);
                    $('#NgayCap').val(data.NgayCap ? data.NgayCap.split('T')[0] : '');
                    $('#GhiChu').val(data.GhiChu || '');
                    $('#titleModal').text('Cập nhật thiết bị phòng');
                    $('.btn-submit-format').text('Cập nhật');
                    self.IsUpdate = true;
                    $('#userModal').modal('show');
                } else {
                    tedu.notify(res.message || "Không tìm thấy thiết bị", "warning");
                }
            },
            error: function () {
                tedu.notify("Lỗi khi lấy dữ liệu thiết bị", "error");
            }
        });
    });

    // Xử lý mở modal sửa chữa
    $(document).on('click', '.btn-repair', function () {
        var phongId = $(this).data('phongid');
        var thietBiId = $(this).data('thietbiid');
        if (!phongId || !thietBiId) {
            tedu.notify("Thiếu thông tin thiết bị/phòng", "warning");
            return;
        }

        $("#repairPhongId").val(phongId);
        $("#repairThietBiId").val(thietBiId);
        $("#repairNgayTao").val(new Date().toISOString().slice(0, 16));
        $("#repairNguoiSua").val("");
        $("#repairChiPhi").val("");
        $("#repairMoTa").val("");
        $("#repairNgayHoanTat").val("");

        $("#modalSuaChua").modal("show");
    });

    // Xử lý xóa thiết bị
    $(document).on('click', '.btn-delete', function () {
        var phongId = $('#phongSelect').val();
        var maThietBi = $(this).data('id');
        if (!phongId) {
            tedu.notify("Vui lòng chọn phòng", "warning");
            return;
        }

        if (confirm("Bạn có chắc muốn xóa thiết bị này khỏi phòng?")) {
            $.ajax({
                url: '/Admin/ThietBiPhong/DeleteThietBiPhong',
                type: 'POST',
                data: { phongId: phongId, thietBiId: maThietBi },
                success: function (res) {
                    if (res.success) {
                        tedu.notify("Xóa thành công", "success");
                        self.loadThietBiTheoPhong(phongId);
                    } else {
                        tedu.notify(res.message || "Xóa thất bại", "error");
                    }
                },
                error: function () {
                    tedu.notify("Lỗi khi xóa thiết bị", "error");
                }
            });
        }
    });

    // Gửi dữ liệu form thiết bị phòng
    self.Submit = function () {
        var phongId = $('#MaPhong').val();
        var maThietBi = $('#MaThietBi').val();

        if (!phongId || !maThietBi) {
            tedu.notify("Thiếu thông tin thiết bị/phòng", "warning");
            return;
        }

        var formData = {
            MaPhong: parseInt(phongId),
            MaThietBi: parseInt(maThietBi),
            NgayCap: $('#NgayCap').val(),
            GhiChu: $('#GhiChu').val()
        };

        $.post('/Admin/ThietBiPhong/AddOrUpdate', formData, function (res) {
            if (res.success) {
                $('#userModal').modal('hide');
                tedu.notify(res.message || "Lưu thành công", "success");
                self.loadThietBiTheoPhong(phongId);
            } else {
                tedu.notify(res.message || "Lưu thất bại", "error");
            }
        }).fail(function () {
            tedu.notify("Lỗi khi lưu dữ liệu", "error");
        });
    };

    // Validate và submit form
    self.Validate = function () {
        $("#form-submit").validate({
            rules: {
                MaThietBi: { required: true },
                NgayCap: { required: true }
            },
            messages: {
                MaThietBi: { required: "Vui lòng chọn thiết bị" },
                NgayCap: { required: "Vui lòng nhập ngày cấp" }
            },
            submitHandler: function () {
                self.Submit();
            }
        });
    };

    // Reset form khi đóng modal
    $('.modal').on('hidden.bs.modal', function () {
        $(this).find('form')[0].reset();
        $("form").validate().resetForm();
        $("label.error").hide();
        $(".error").removeClass("error");
    });

    // Gửi yêu cầu sửa chữa thiết bị
    window.hoanTatSuaChua = function () {
        const data = {
            MaPhong: $("#repairPhongId").val(),
            MaThietBi: $("#repairThietBiId").val(),
            NgayTao: $("#repairNgayTao").val(),
            TenNguoiSua: $("#repairNguoiSua").val(),
            TienSua: $("#repairChiPhi").val(),
            Comment: $("#repairMoTa").val(),
            NgayHoanTat: new Date().toISOString()
        };

        $.post("/Admin/SuaChua/HoanTatSuaChua", data, function (res) {
            if (res.success) {
                tedu.notify("Sửa chữa thành công và đã thêm vào hóa đơn!", "success");
                $("#modalSuaChua").modal("hide");
                $('#btnSearch').click();
            } else {
                tedu.notify(res.message || "Lỗi khi xử lý sửa chữa", "error");
            }
        });
    };

    // Khởi tạo
    $(document).ready(function () {
        self.Validate();
        $('#btnSearch').click();
    });

})(jQuery);