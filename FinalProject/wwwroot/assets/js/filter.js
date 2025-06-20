 const totalPages = 10;
  let currentPage = 1;

  function renderPagination() {
    const pagination = document.getElementById('pagination');
    pagination.innerHTML = '';

    const createPageItem = (page, label = null, isActive = false, isDisabled = false) => {
      const li = document.createElement('li');
      li.className = `page-item${isActive ? ' active' : ''}${isDisabled ? ' disabled' : ''}`;
      const a = document.createElement('a');
      a.className = 'page-link';
      a.href = '#';
      a.textContent = label || page;
      a.addEventListener('click', (e) => {
        e.preventDefault();
        if (!isDisabled && page !== '...') {
          currentPage = page;
          renderPagination();
        }
      });
      li.appendChild(a);
      return li;
    };

    // Назад
    pagination.appendChild(createPageItem(currentPage - 1, 'Назад', false, currentPage === 1));

    // Всегда показываем первые 3 страницы
    for (let i = 1; i <= Math.min(3, totalPages); i++) {
      pagination.appendChild(createPageItem(i, null, i === currentPage));
    }

    // Умная логика: если текущая страница больше 2, показываем 4-6
    if (currentPage > 2 && totalPages > 6) {
      for (let i = 4; i <= Math.min(6, totalPages - 1); i++) {
        pagination.appendChild(createPageItem(i, null, i === currentPage));
      }
    }

    // Многоточие
    if (totalPages > 6 && currentPage <= 2) {
      const dots = createPageItem('...', '...');
      dots.classList.add('disabled');
      pagination.appendChild(dots);
    } else if (totalPages > 6 && currentPage >= 3 && currentPage < totalPages - 2) {
      const dots = createPageItem('...', '...');
      dots.classList.add('disabled');
      pagination.appendChild(dots);
    }

    // Последняя страница
    if (totalPages > 6 && currentPage < totalPages) {
      pagination.appendChild(createPageItem(totalPages, null, currentPage === totalPages));
    }

    // Вперёд
    pagination.appendChild(createPageItem(currentPage + 1, 'Вперёд', false, currentPage === totalPages));
  }

  renderPagination();