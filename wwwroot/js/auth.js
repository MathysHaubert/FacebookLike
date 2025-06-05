window.loginUser = async function(username, password) {
    try {
        const response = await fetch('/api/auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ username, password })
        });
        let data = null;
        try {
            const text = await response.text();
            data = text ? JSON.parse(text) : null;
        } catch (e) {
            data = null;
        }
        if (!response.ok) {
            return { success: false, status: response.status, message: data?.message || 'Incorrect username or password' };
        }
        return data || { success: true };
    } catch (error) {
        return { success: false, message: error.message };
    }
};

window.logoutUser = async function() {
    try {
        const response = await fetch('/api/auth/logout', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            credentials: 'include'
        });
        if (!response.ok) {
            return { success: false, status: response.status, message: 'Logout failed' };
        }
        return { success: true };
    } catch (error) {
        return { success: false, message: error.message };
    }
};

