function getCookie(name) {
    const match = document.cookie.match(new RegExp('(^| )' + name + '=([^;]+)'));
    return match ? match[2] : null;
}

const api = axios.create({
    baseURL: 'https://localhost:7144/api' 
});

api.interceptors.request.use(config => {
    const token = getCookie('focusvisk_api_token');
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

api.interceptors.response.use(
    response => response,
    error => {
        if (error.response?.status === 401) {
            window.location.href = '/Account/Login';
        }
        return Promise.reject(error);
    }
);