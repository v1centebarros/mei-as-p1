import {QueryClient, QueryClientProvider} from "@tanstack/react-query"
import Router from "./utils/router"
import {AuthProvider} from "./contexts/Auth.jsx";

function App() {

    const queryClient = new QueryClient()
    return (
        <QueryClientProvider client={queryClient}>
            <AuthProvider>
                <Router/>
            </AuthProvider>
        </QueryClientProvider>
    )
}

export default App
