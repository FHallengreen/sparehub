import React, { createContext, useContext, useState, ReactNode, useEffect } from 'react';

interface AuthContextType {
  user: { id: number; email: string; role: string } | null;
  token: string | null;
  login: (userData: { id: number; email: string; role: string }, token: string) => void;
  logout: () => void;
  isAuthenticated: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  const [user, setUser] = useState<{ id: number; email: string; role: string } | null>(null);
  const [token, setToken] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const storedSession = localStorage.getItem('session');
    if (storedSession) {
      const { user: storedUser, token: storedToken } = JSON.parse(storedSession);
      setUser(storedUser);
      setToken(storedToken);
    }
    setLoading(false); 
  }, []);

  const login = (userData: { id: number; email: string; role: string }, authToken: string) => {
    setUser(userData);
    setToken(authToken);
    localStorage.setItem('session', JSON.stringify({ user: userData, token: authToken }));
  };

  const logout = () => {
    setUser(null);
    setToken(null);
    localStorage.removeItem('session');
  };

  const isAuthenticated = !!user && !!token;

  if (loading) {
    return <div>Loading...</div>;
  }

  return (
    <AuthContext.Provider value={{ user, token, login, logout, isAuthenticated }}>
      {children}
    </AuthContext.Provider>
  );
};


export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
