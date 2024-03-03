import {QueryClient, QueryClientProvider} from "@tanstack/react-query"
import Router from "./utils/router"
import {AuthProvider} from "./contexts/Auth.jsx";
import {ReactQueryDevtools} from "@tanstack/react-query-devtools";

function App() {

    const queryClient = new QueryClient()
    return (
        <QueryClientProvider client={queryClient}>
            <AuthProvider>
                <Router/>
            </AuthProvider>
            <ReactQueryDevtools/>
        </QueryClientProvider>
    )
}

export default App
